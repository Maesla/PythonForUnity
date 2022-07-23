import time
from System import Action
import math
import numpy as np

print('1')
time.sleep(1)
print('2')
time.sleep(1)
print('3')


# def Create(x):
    # go = GameObject.CreatePrimitive(PrimitiveType.Sphere)
    # go.transform.position = Vector3(float(x), 0.0, 0.0)
    
# callback = Action[str](PrintText)

#d = Action(lambda:GameObject.CreatePrimitive(PrimitiveType.Sphere))
#d = Action(Create)

def Move(y, go):
    go.transform.position = Vector3(0.0, y, 0.0)


# for x in range(6):
  # d = Action(lambda:Create(x))
  # unityEngine.RunThread(d)
  # time.sleep(1)
  
  
go = None
def Create2():
    global go
    go = GameObject.CreatePrimitive(PrimitiveType.Sphere)

unityEngine.RunThread(Action(Create2))


for y in np.arange(0,10*2*math.pi, 0.1):
  print(y)
  d = Action(lambda:Move(math.sin(y), go))
  unityEngine.RunThread(d)
  time.sleep(0.01)
  