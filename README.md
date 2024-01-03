# 📌 Behaviour Tree
유한 상태 머신 (Finite State Machine) 이나 AI프로그래밍에 사용되는 기타 시스템과 달리 BT는 AI의 의사 결정 흐름을 제어하는 계층적인 노드 트리이다. Tree 범위에서 leaf(Action)은 AI 개체를 제어하는 실제 명령이고 가지를 형성하는 것은 상황에 가장 적합한 명령 시퀸스에 도달하기 위해 AI가 나무(Tree)따라 내려가는 것을 제어하는 다양한 유형의 유틸리티 노드이다.

## ➔ 탐색 순서
Behaviour Tree는 트리 구조이기 때문에, **위에서 아래로, 왼쪽에서 오른쪽 순**으로 진행된다.

## ➔ 노드 구조
보통은 **Leaf(Action), Selector, Sequence** 3개의 노드를 기본적으로 가지고 있다.

## ➔ 노드 상태
각 노드들은 자신의 상태를 반환해야 한다.
어떻게 구성하느냐에 따라 다르지만 대부분 아래 3가지로 구분한다.

- Failure (실패)
- Running (동작 중)
- Success (성공)

<br/>
<br/>

***

<br/>
<br/>

# 📌 Node

## ➔ Leaf(Atcion Node)
Leaf(Action Node)는 나뭇잎, 트리의 끝 구조 즉, **행동을 정의한 노드** 이다.

![](https://velog.velcdn.com/images/ljun970/post/71c85da7-a8a0-4153-a8d1-c60d036018e0/image.png)


## ➔ Selector Node
- **or 연산자**와 동일한 기능을 하는 노드이다.

- 자식 노드들을 왼쪽에서 오른쪽 순으로 진행.
	
    - **우선순위**가 **높은** 자식 노드일수록 **왼쪽**에 배치되어야 한다.
    - 자식 노드들 중에서 **성공한 노드**가 있다면 그 노드를 **실행**하고 **종료**한다.
    - 여기에서 성공이란, Success / Running을 뜻한다.
    
- 여러 행동 중 **하나만 실행**해야 할 때 사용하기 좋다.

![](https://velog.velcdn.com/images/ljun970/post/0bd580b9-932a-4edb-b511-801cba75dea4/image.png)


## ➔ Sequence Node
- **and 연산자**와 동일한 기능을 한다.

- 자식 노드들을 왼쪽에서 오른쪽 순으로 진행한다.
	- 먼저 진행해야 할 자식 노드가 **왼쪽**에 위치해야 한다.
    - 자식 노드들 중에서 **실패(Failure)한** 노드가 있을 때까지 진행한다.
    
- 여러 행동을 **순서대로 진행**해야 할 때 사용하기 좋다.

![](https://velog.velcdn.com/images/ljun970/post/d4172bc6-5e52-4270-aee5-2f3519c2e124/image.png)


<br/>
<br/>
<br/>
<br/>
<br/>

***

<br/>
<br/>
<br/>
<br/>
<br/>

# 📌 Node 구현 (C#)

![TreeBT](https://github.com/GreatJun/Test-AI/assets/127035454/8d3d24fc-33a1-420d-b600-147c54c8485b)


## ➔ INode (interface)
> 노드의 통일성을 위해서 인터페이스 INode를 만들어 준다.
인터페이스에서 Node의 상태와 노드가 어떤 상태인지를 반환하는 Evaluate() 메소드를 추가한다.


## ➔ Action(Leaf) Node
> **Action Node**는 **실제로 어떤 행위**를 하는 노드이다.
그렇기 때문에 Func() 델리게이트를 통해 행위를 전달받아 실행한다.
> 

## ➔ Selector Node
> Selector Node는 **자식 노드** 중에서 **처음**으로 **Success** 또는 **Running 상태**를 가진 노드가 발생하면 그 노드까지 **진행하고 멈춘다**.

- 자식 상태 : Running일 때 ➔ Running 반환

- 자식 상태 : Success일 때 ➔ Success 반환

- 자식 상태 : Failure일 때 ➔ 다음 자식으로 이동


## ➔ Sequence Node
> Sequence Node는 자식 노드를 왼쪽에서 오른쪽으로 진행하면서 **Failure 상태가 나올 때까지 진행**한다.

- 자식 상태 : Running일 때 ➔ Running 반환

- 자식 상태 : Success일 때 ➔ 다음 자식으로 이동

- 자식 상태 : Failure일 때 ➔ Failure 반환


### Sequence Node 주의점
> Running 상태일 때는 그 상태를 유지해야 하기 때문에 다음 자식 노드로 이동하면 안 되고
다음 프레임 때도 그 자식에 대한 평가를 진행해야 한다.

예를 들어, Sequence Node에 적 발견(Detect), 이동(Move), 공격(Attack) 총 3개의 자식 노드가 있다고 가정해 본다면
``프레임마다 노드에 진입하는 상황은 "N차"로 가정``

- 1차 : 적을 발견하고 적을 향해 이동한다.
- 2차 : 발견한 적을 향해 아직 이동 중이다.
- 3차 : 발견한 적을 향해 아직 이동 중이다.
- 4차 : 이동이 완료되어 적을 공격한다.

만역 이 때, running에서 반환되지 않고 다음 자식 노드로 이동하게 된다면?
아직 적에게 다가가지 못했는데 적을 향해 공격하게 될 것이다.

그러므로, Running 상태에서는 Success와 다르게 다음 자식으로 이동하지 않고 Running을 반환해 줘서 다음 진입 시에도 Running 상태를 유지할 수 있도록 해주어야 한다.

![](https://velog.velcdn.com/images/ljun970/post/9d97f59a-49b7-450c-b8bf-a20afffed92d/image.png)


