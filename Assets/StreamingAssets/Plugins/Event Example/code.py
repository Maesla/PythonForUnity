from System import Action

def PrintText(text):
    print(text)
    
callback = Action[str](PrintText)

eventProvider.Load(callback)