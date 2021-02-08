# Course-PostProcessingEffect

屏幕后处理效果（Screen Post Processing Effects），是游戏中实现屏幕特效的方法，有助于提升画面效果。[《屏幕后处理效果系列之图像模糊算法篇》](https://edu.uwa4d.com/course-intro/1/280)主要讲解的是图像模糊算法。在游戏中经常被应用的屏幕后处理特效，例如炫光（Bloom）、景深（Depth of Field）、镜头光晕（Glare Lens Flare）、体积光（Volume Ray）等效果，都用到了图像模糊算法。

从图像处理领域的角度来看，图像模糊算法是一种低通滤波算法。经过低通滤波器处理后的图像效果看起来像是将图像变模糊了，故而被应用到屏幕后处理特效中。图像模糊算法有很多经典算法，本篇教程介绍了高斯模糊算法（Gaussian Blur）、盒式模糊（Box Blur）、Kawase Blur和Dual Blur。在本篇中将结合Demo学习相关算法及其优化算法，探究其应用与优化方式。

## 目录

本篇教程介绍了如下四种算法。

[高斯模糊（第1-3节）](https://edu.uwa4d.com/lesson-detail/280/1297/0?isPreview=false)

[盒式模糊（第4节）](https://edu.uwa4d.com/lesson-detail/280/1306/0?isPreview=0)

[Kawase Blur（第5节）](https://edu.uwa4d.com/lesson-detail/280/1307/0?isPreview=0)

[Dual Blur（第6节）](https://edu.uwa4d.com/lesson-detail/280/1315/0?isPreview=0)

***

### 高斯模糊

#### [1）基础算法及其实现](https://edu.uwa4d.com/lesson-detail/280/1297/0?isPreview=false)
高斯模糊（Gaussian Blur）又名高斯平滑（Gaussian Smoothing），是一个图像模糊的经典算法。简单来说，高斯模糊算法就是对整幅图像进行加权平均运算的过程，每一个像素点的值，都是由其本身和领域内的其他像素值经过加权平均后得到。

**在Unity的实现**

原图：
<center><img src="https://github.com/UWA-MakeItSimple/Course-PostProcessingEffect/blob/main/Assets/Shaders/Blur/GaussianBlur/13.jpg" style="width:500px"></center> 
使用高斯核大小为7\*7：
<center><img src="https://github.com/UWA-MakeItSimple/Course-PostProcessingEffect/blob/main/Assets/Shaders/Blur/GaussianBlur/14.png" style="width:500px"></center> 
使用高斯核大小为35\*35：
<center><img src="https://github.com/UWA-MakeItSimple/Course-PostProcessingEffect/blob/main/Assets/Shaders/Blur/GaussianBlur/15.png" style="width:500px"></center> 

#### [2）分步两次一维运算算法及其实现](https://edu.uwa4d.com/lesson-detail/280/1298/0?isPreview=0)
使用二维高斯函数计算得到的卷积核对图像进行模糊处理，应用在实时计算的游戏中开销较大。需要考虑一些方法进行优化加速，减少贴图读取的操作和算术运算的次数。可以利用二维高斯分布函数的可分离特性来进行优化。

**在Unity的实现**
使用高斯核大小为35\*35：
<center><img src="https://github.com/UWA-MakeItSimple/Course-PostProcessingEffect/blob/main/Assets/Shaders/Blur/GaussianBlur/23.png" style="width:500px"></center> 

#### [3）采用线性采样的算法及其实现](https://edu.uwa4d.com/lesson-detail/280/1299/0?isPreview=0)
通过利用高斯分布函数的特性，成功地减少了贴图读取次数和算数运算次数。可以利用GPU的优势，进行进一步地提速。此前的算法都是假设了一次贴图读取只能用来获得一个像素的信息，但对于GPU来说却并不总是这样，当图片读取开启双线性插值采样（Bilinear Sampling）的时候，GPU可以一次读取多个像素信息，而GPU使用双线性插值并没有什么额外的负担。

**在Unity的实现**
使用高斯核大小为35\*35：
<center><img src="https://github.com/UWA-MakeItSimple/Course-PostProcessingEffect/blob/main/Assets/Shaders/Blur/GaussianBlur/26.png" style="width:500px"></center>

