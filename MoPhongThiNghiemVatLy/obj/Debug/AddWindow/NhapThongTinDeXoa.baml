   M S B A M L   `   `   `  ����P  LMoPhongThiNghiemVatLy, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullQ-clr-namespace:MoPhongThiNghiemVatLy.AddWindowMoPhongThiNghiemVatLy.AddWindow  5   	   7    1MoPhongThiNghiemVatLy.AddWindow.NhapThongTinDeXoa   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   ?local-clr-namespace:MoPhongThiNghiemVatLy.AddWindow     5   	   _ [MaterialDesignThemes.Wpf, Version=1.0.1.0, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5   	     � Title$  Xóa thành phần��5	   	   $	��320��5
   	   $	��370��5   	    � 
ResizeMode$ NoResize=�5   	    +� FocusedElement 5   	   ��  �� ElementName$ PTextBox����5      �� 5   
    �� 
StartPoint$	 0,0&�6    �� EndPoint$	 1,0&�5      .��5      �  � Color$ #D7B9FF��6    � Offset$ 0Q�5      � 5      $ #C2E8FF��6   $ 1Q�5      5      .��5      �  � RowDefinitions 5   
   �� 5      $��0.25* �6   �� 5       $��0.15* �6   �� 5"      $��0.20* �6   �� 5$      $��0.20* �6   �� 5&      $��0.20* �6   .+�5*   
   � $��0��6   $��2��5+      .+�5,      (� $��Center=�6   $��Center=�5-      $��60��5.      $��60��5/      .��50      � .+�52      A� $��80��6   $��80��53      
����������#D7B9FF$��3��56      �� 58      $	��DEL��6    $��30(�59       $
��Bold"�5:       
����p��$��Center=�5<       $��Center=�5=       5>      5?      5@      � 5C   
   $��1��6   .+�5D      �� $#��Nhập thông tin cần xóa:��6   $��Center=�5E      $��Bottom=�5F      $��20(�5G      $
��Bold"�5H      
���   �5I      5J      � 5M   
   $��2��6   .+�5N      � 	 � ColumnDefinitions	 5P      �� 5Q      $
��0.4* �6'   �� 5R      $
��0.6* �6'   .+�5V      '  !MaterialDesignThemes.Wpf.PackIcon  
   Kind$
 HelpCircleOutline=�6*   $��25(�5W   *   $��30��5X   *   $��30��5Y   *   $��Right=�5Z   *   $��Center=�5[   *   $��0,0,15,0q�5\   *   
���   �5]   *   �� 5_      -   5         ��Name$ ComponentTypeComboBox��5_      $
��Left=�5`      $	��103��5a      $��30��5b      $��16(�5c      
�������
���   �$��Center=�5f      $��Center=�5g      )  #MaterialDesignThemes.Wpf.HintAssist   Hint$ Loại��5i      $��1��5j      ��5h      �� *ComponentTypes+5j      5k      5l      � 5o   
   $��3��6   .+�5p      � 	 5r      �� 5s      $
��0.4* �6'   �� 5t      $
��0.6* �6'   .+�5x        $
 FormatListNumbered=�6*   $��25(�5y   *   $��38��5z   *   $��30��5{   *   $��Right=�5|   *   $	��Top=�5}   *   $��0,0,15,0q�5~   *   
���   �5   *   �� 5�      -   5        $ IndexComboBox��5�      $
��Left=�5�      $	��103��5�      $��30��5�      $��16(�5�      
�������
���   �$	��Top=�5�      $��Center=�5�      $ 
Chỉ số��5�      $��1��5�      $��0,4,0,0q�5�      ��5�      �� *LongIntegerList+5�      5�      5�      �� 5�   
    �� Orientation$ 
Horizontal=�6   $��Center=�5�      $	��Top=�5�      $��4��5�      .+�5�      �� -   5        $
��Save��5�      $	��100��5�      $��40��5�      $��0,0,10,0q�5�      
����p��
�������$��16(�5�      5�      �� 5�      -   5        $��Cancel��5�      $	��100��5�      $��40��5�      
�������
����p��$��16(�5�      5�      5�      5�      5�      