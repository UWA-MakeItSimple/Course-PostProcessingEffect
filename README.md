# Course-PostProcessingEffect

屏幕后处理效果（Screen Post Processing Effects），是游戏中实现屏幕特效的方法，有助于提升画面效果。UWA学堂将推出一系列课程，带领读者逐步掌握各种屏幕后处理效果的理论基础和实现方式。

[《屏幕后处理效果系列之图像模糊算法篇》](https://edu.uwa4d.com/course-intro/1/280)主要讲解的是**图像模糊算法**。在游戏中经常被应用的屏幕后处理特效，例如炫光（Bloom）、景深（Depth of Field）、镜头光晕（Glare Lens Flare）、体积光（Volume Ray）等效果，都用到了图像模糊算法。

从图像处理领域的角度来看，图像模糊算法是一种低通滤波算法。经过低通滤波器处理后的图像效果看起来像是将图像变模糊了，故而被应用到屏幕后处理特效中。图像模糊算法有很多经典算法，本篇教程介绍了高斯模糊算法（Gaussian Blur）、盒式模糊（Box Blur）、Kawase Blur和Dual Blur。在本篇中将结合Demo学习相关算法及其优化算法，探究其应用与优化方式。

[《屏幕后处理效果系列之常见后处理效果篇》](https://edu.uwa4d.com/course-intro/0/285)讲解的是**常见后处理效果**，将结合Demo学习在游戏中经常被应用的屏幕后处理特效，例如眩光（Bloom）、景深（Depth of Field）、镜头光晕（Lens Flare）等效果的相关实现方式及其实际项目应用中的优化方式。



## 目录

#### 一、图像模糊算法

[1. 高斯模糊（第1-3节）](https://edu.uwa4d.com/lesson-detail/280/1297/0?isPreview=false)

[2. 盒式模糊（第4节）](https://edu.uwa4d.com/lesson-detail/280/1306/0?isPreview=0)

[3. Kawase Blur（第5节）](https://edu.uwa4d.com/lesson-detail/280/1307/0?isPreview=0)

[4. Dual Blur（第6节）](https://edu.uwa4d.com/lesson-detail/280/1315/0?isPreview=0)

#### 二、常见的后处理效果

[1. Real-Time Glow & Bloom（实时辉光）](https://edu.uwa4d.com/lesson-detail/285/1329/0?isPreview=false)

[2. Depth Of Field（景深）](https://edu.uwa4d.com/lesson-detail/285/1330/0?isPreview=0)

[3. Lens Flare —— Streak（拉丝）](https://edu.uwa4d.com/lesson-detail/285/1331/0?isPreview=0)

[4. Silhouette Rendering（轮廓渲染）](https://edu.uwa4d.com/lesson-detail/285/1347/0?isPreview=0)

[5. Radial Blur（径向模糊）](https://edu.uwa4d.com/lesson-detail/285/1348/0?isPreview=0)


***



## 一、图像模糊算法

### 1. 高斯模糊

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

### [2. 盒式模糊（Box Blur）](https://edu.uwa4d.com/lesson-detail/280/1306/0?isPreview=0)

盒式模糊（Box Blur）算法，同样使用了卷积运算，而它使用的卷积核每个位置具有相同的权重。

**在Unity的实现**

我们选择36x36大小的卷积核得到结果：

<center><img src="https://github.com/UWA-MakeItSimple/Course-PostProcessingEffect/blob/main/%E6%95%88%E6%9E%9C%E5%9B%BE/boxblur.png" style="width:500px"></center>

可以看出，上图有很明显的马赛克感，再进行一次Box Blur处理：

<center><img src="https://github.com/UWA-MakeItSimple/Course-PostProcessingEffect/blob/main/%E6%95%88%E6%9E%9C%E5%9B%BE/boxblur1.png" style="width:500px"></center>

Box Blur算法的最大优势在于，它能将处理一个元素的时间复杂度降为常数，使其与卷积核的大小不再有关系。但是，它的固定成本却是很高的，而且由于用到了Compute Shader，有一些设备不支持这种算法。只有当使用的卷积核大小超过某个阈值，这种算法的性能优势才会显现出来。因此它可能只适用于一些特定情况。

### [3. Kawase Blur](https://edu.uwa4d.com/lesson-detail/280/1307/0?isPreview=0)

Box Blur算法的优化思路是针对时间复杂度，将时间复杂度降到常数级。而Kawase Blur算法则是将目光对准硬件层面，充分发挥GPU进行双线性插值采样的开销较小的优势，来进行算法优化。

**在Unity的实现**

根据Kawase Blur算法得到结果：

<center><img src="https://github.com/UWA-MakeItSimple/Course-PostProcessingEffect/blob/main/%E6%95%88%E6%9E%9C%E5%9B%BE/kawase.png" style="width:500px"></center>

35x35 Gaussian Blur 的结果：

<center><img src="https://github.com/UWA-MakeItSimple/Course-PostProcessingEffect/blob/main/%E6%95%88%E6%9E%9C%E5%9B%BE/kawase1.png" style="width:500px"></center>

可以看到这样处理得到的结果与具有35x35卷积核Gaussian Blur得到的结果相近。

Kawase Blur算法的优势在于最大程度上利用里GPU的硬件特性，这使得该算法的性能十分优越。但是Kawase Blur算法的模糊效果质量的估算是一个需要不断尝试的经验值，之前的算法有固定的卷积核和模糊次数可以衡量模糊效果质量的好坏，卷积核大小越大、模糊次数越多效果越好。而Kawase Blur设定的偏移量并不会像算子大小一样对模糊效果产生质量影响。所以为了尽量达到高斯模糊运算的效果，会选择几个不同的偏移值，进行多次模糊运算。

### [4. Dual Blur](https://edu.uwa4d.com/lesson-detail/280/1315/0?isPreview=0)

Dual Blur（双滤波模糊）是基于Kawase Blur的改进算法，该算法在充分利用GPU硬件特性的情况下，采用下采样（Down-Sampling）缩小图片、上采样（Up-Sampling）放大图片的方式进一步减少贴图读取次数。

**在Unity的实现**

<center><img src="https://github.com/UWA-MakeItSimple/Course-PostProcessingEffect/blob/main/%E6%95%88%E6%9E%9C%E5%9B%BE/Dualblur.png" style="width:500px"></center>



## 二、常见的后处理效果

### [1. Real-Time Glow & Bloom（实时辉光）](https://edu.uwa4d.com/lesson-detail/285/1329/0?isPreview=false)

Bloom（Glow）特效是游戏中应用最普遍的一种屏幕后处理特效。表现为高光物体带有泛光效果，使画面的光影表现更加优秀。Bloom通常会搭配HDR和ToneMapping来得到更好的效果。

**在Unity的实现**

得到效果如图：
<center><img src="https://github.com/UWA-MakeItSimple/Course-PostProcessingEffect/blob/main/%E6%95%88%E6%9E%9C%E5%9B%BE/realtime.png" style="width:500px"></center>

原图如下：
<center><img src="https://github.com/UWA-MakeItSimple/Course-PostProcessingEffect/blob/main/%E6%95%88%E6%9E%9C%E5%9B%BE/realtime1.png" style="width:500px"></center>

### [2. Depth Of Field（景深）](https://edu.uwa4d.com/lesson-detail/285/1330/0?isPreview=0)

Depth Of Field（景深）是游戏中常用的屏幕后处理特效之一。它来自于摄影中的一个基础概念，是指在摄影机镜头或其他成像器前沿能够取得清晰图像的成像所测定的被摄物体前后距离范围。处在景深范围内的物体，成像清晰；处在景深范围外的物体，成像模糊。

**在Unity的实现**

得到效果如图所示：
<center><img src="https://github.com/UWA-MakeItSimple/Course-PostProcessingEffect/blob/main/%E6%95%88%E6%9E%9C%E5%9B%BE/depthoffield.png" style="width:500px"></center>
<center><img src="https://github.com/UWA-MakeItSimple/Course-PostProcessingEffect/blob/main/%E6%95%88%E6%9E%9C%E5%9B%BE/depthoffield1.png" style="width:500px"></center>

### [3. Lens Flare —— Streak（拉丝）](https://edu.uwa4d.com/lesson-detail/285/1331/0?isPreview=0)

摄影界有一种特殊的滤镜是Streak Filters，以发光点为中心，向四周发散一系列平行线条，从而产生一种光芒四射的效果。在游戏中，这也是一种常见的效果，来展示发光点的高光，烘托气氛。这种效果实现方式，本节我们采用一种较为简单的基于Dual Blur模糊算法思想的方法实现它。

**在Unity的实现**

使用一个自发光得球形，施加一个高光，得到效果：
<center><img src="https://github.com/UWA-MakeItSimple/Course-PostProcessingEffect/blob/main/%E6%95%88%E6%9E%9C%E5%9B%BE/streak.png" style="width:500px"></center>

调高平行光强度，得到效果：
<center><img src="https://github.com/UWA-MakeItSimple/Course-PostProcessingEffect/blob/main/%E6%95%88%E6%9E%9C%E5%9B%BE/streak1.png" style="width:500px"></center>

### [4. Silhouette Rendering（轮廓渲染）](https://edu.uwa4d.com/lesson-detail/285/1347/0?isPreview=0)

轮廓渲染是一种常见的视觉效果，也称之为描边（Outline），常出现在非真实感渲染中。像《无主之地》系列这样漫画风格比较强烈的游戏中，运用了大量的轮廓渲染。

**在Unity的实现**

只绘制轮廓：
<center><img src="https://github.com/UWA-MakeItSimple/Course-PostProcessingEffect/blob/main/%E6%95%88%E6%9E%9C%E5%9B%BE/outline.png" style="width:500px"></center>
混合原图像进行描边：
<center><img src="https://github.com/UWA-MakeItSimple/Course-PostProcessingEffect/blob/main/%E6%95%88%E6%9E%9C%E5%9B%BE/outline1.png" style="width:500px"></center>
原图：
<center><img src="https://github.com/UWA-MakeItSimple/Course-PostProcessingEffect/blob/main/%E6%95%88%E6%9E%9C%E5%9B%BE/outline2.png" style="width:500px"></center>
调节轮廓颜色：
<center><img src="https://github.com/UWA-MakeItSimple/Course-PostProcessingEffect/blob/main/%E6%95%88%E6%9E%9C%E5%9B%BE/outline3.png" style="width:500px"></center>

### [5.Radial Blur（径向模糊）](https://edu.uwa4d.com/lesson-detail/285/1348/0?isPreview=0)

Radial Blur（径向模糊）是一种常见的视觉效果，具体表现为从中心向外呈辐射条状模糊。

得到效果如图：
<center><img src="https://github.com/UWA-MakeItSimple/Course-PostProcessingEffect/blob/main/%E6%95%88%E6%9E%9C%E5%9B%BE/radial.png" style="width:500px"></center>
动态调节变量_BlurRadius：
<center><img src="https://github.com/UWA-MakeItSimple/Course-PostProcessingEffect/blob/main/%E6%95%88%E6%9E%9C%E5%9B%BE/radial1.gif" style="width:500px"></center>
