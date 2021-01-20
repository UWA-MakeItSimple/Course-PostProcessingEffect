# Course-PostProcessingEffect

屏幕后处理效果（Screen Post Processing Effects），是游戏中实现屏幕特效的方法，有助于提升画面效果。[《屏幕后处理效果系列之图像模糊算法篇》](https://edu.uwa4d.com/course-intro/1/280)主要讲解的是图像模糊算法。在游戏中经常被应用的屏幕后处理特效，例如炫光（Bloom）、景深（Depth of Field）、镜头光晕（Glare Lens Flare）、体积光（Volume Ray）等效果，都用到了图像模糊算法。

从图像处理领域的角度来看，图像模糊算法是一种低通滤波算法。经过低通滤波器处理后的图像效果看起来像是将图像变模糊了，故而被应用到屏幕后处理特效中。图像模糊算法有很多经典算法，例如：高斯模糊算法（Gaussian Blur）、盒式模糊（Box Blur）、Kawase Blur、径向模糊（Radial Blur）、散景模糊（Bokeh Blur）等。在本篇中将结合Demo学习相关算法及其优化算法，探究其应用与优化方式。

#### 高斯模糊算法
#### 盒shi糊

## 目录

本篇教程采用连载的方式更新（更新至Kawase Blur）。
[高斯模糊（第1-3节）](https://edu.uwa4d.com/lesson-detail/280/1297/0?isPreview=false)
[盒式模糊（第4节）](https://edu.uwa4d.com/lesson-detail/280/1306/0?isPreview=0)
[Kawase Blur（第5节）](https://edu.uwa4d.com/lesson-detail/280/1307/0?isPreview=0)
径向模糊
散景模糊

***

### 高斯模糊

1）基础算法及其实现
高斯模糊（Gaussian Blur）又名高斯平滑（Gaussian Smoothing），是一个图像模糊的经典算法。简单来说，高斯模糊算法就是对整幅图像进行加权平均运算的过程，每一个像素点的值，都是由其本身和领域内的其他像素值经过加权平均后得到。

原图：
