import base64
import io
from PIL import Image
import matplotlib.pyplot as plt
import numpy as np

mu, sigma = 100, 15
x = mu + sigma * np.random.randn(10000)

# the histogram of the data
n, bins, patches = plt.hist(x, 50, density=1, facecolor='g', alpha=0.75)


plt.xlabel('Smarts')
plt.ylabel('Probability')
plt.title('Histogram of IQ')
plt.text(60, .025, r'$\mu=100,\ \sigma=15$')
plt.axis([40, 160, 0, 0.03])
plt.grid(True)

buf = io.BytesIO()
plt.savefig(buf, format='png')
buf.seek(0)

view = buf.getvalue()
view2 = base64.b64encode(buf.getvalue()).decode()
#print(view)
#print(view2)
#im = Image.open(buf)
#im.show()
buf.close()
plt.close()

imageProvider.SetImage(view2)