   M S B A M L   `   `   `  ����I  EExercise Timer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null/clr-namespace:Exercise_TimerExercise_Timer  5   	        Exercise_Timer.TimerWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   :xctk)http://schemas.xceed.com/wpf/xaml/toolkit     5   	   .localclr-namespace:Exercise_Timer     5   	     � Title$  TimerWindow��5	   	   $	��305��6   $	��420��6*    � 
ResizeMode$ NoResize=�66   .��5
      �� .+�5   
   �� -   5        $��
CheckFloor��5      $��20, 20q�6%   $��DemiBold"�65   .��6K   Floor � LayoutTransform 5      �� 5       �� ScaleX$ 2Q�6!    �� ScaleY$ 2Q�6,   5      �� 5   
   -   5        $��	CheckWall��5      $��150, 20q�6$   $��DemiBold"�65   .��6K   Wall 5      �� 5      $ 2Q�6!   $ 2Q�6,   5      �� 5   
   -   5        $��	CheckBand��5      $��280, 20q�6$   $��DemiBold"�65   .��6K   Band 5      �� 5      $ 2Q�6!   $ 2Q�6,   5      �� 5   
   -   5        $��txtTime��5      $��140, 70q�6!   $	��180��62   $��20(�6>   6N   �� 5   
   -   5        $��txtBreak��5      $��140, 120q�6"   $	��180��64   $��20(�6@   6P   �� 5   
   $��25, 65q�6   $��20(�6    $��DemiBold"�6.   .��6F   
Work time:6R   �� 5   
   $��5, 115q�6   $��20(�6    $��DemiBold"�6.   .��6F   Exercise time:6V   �� 5   
   -   5        $��ButtonEnter��5      $��90,170q�6%   $	��150��65   $��80��6A   $��26(�6N   $��DemiBold"�6\   
���   �
�������.��6�   
Continue6�   5      5       