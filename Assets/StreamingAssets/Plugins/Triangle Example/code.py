def CreateTriangle(base, length):
    triangle = triangleProvider.Create(base, length)
    return triangle

triangle = CreateTriangle(3,4)

area = triangle.CalculateArea()

print(area)