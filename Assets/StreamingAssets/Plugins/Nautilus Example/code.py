import math 

def instantiate(parent):
    go = GameObject.CreatePrimitive(PrimitiveType.Cube)
    go.transform.parent = parent
    go.transform.scale = Vector3(0.1, 0.1, 0.1)
    return go


def calculate_position(theta, a, b):
    r = a*math.exp(b*theta)
    x = math.cos(theta)*r
    z = math.sin(theta)*r
    y = r
    return Vector3(x, y, z)
    
def calculate_rotation(theta):
        deg = math.degrees(theta)
        rot = Quaternion.AngleAxis(90-deg, Vector3.up)
        return rot

def create_nautilus_spiral(a, b):
    parent = GameObject("Parent").transform
    for i in range(count):
      go = instantiate(parent)      
      theta = i*step
      go.transform.position = calculate_position(theta, a, b)
      go.transform.rotation = calculate_rotation(theta)


step = 0.1
count = 1000
a = 5
b = 0.1759

create_nautilus_spiral(a,b)