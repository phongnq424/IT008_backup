   M S B A M L   `   `   `  ����  MoPhongThiNghiemVatLy3    -MoPhongThiNghiemVatLy.AddWindow.NhapGTDienTro   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   _ [MaterialDesignThemes.Wpf, Version=1.0.1.0, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5   	     � Title$  Nhập thông tin��5   	   $	��300��5	   	   $	��300��5
   	    � 
ResizeMode$ NoResize=�5   	    +� FocusedElement 5   	   ��  �� ElementName$ QuantityTextBox����5      �� 5   
    �� 
StartPoint$	 0,0&�6    �� EndPoint$	 1,0&�5      .��5      �  � Color$ #D7B9FF��6    � Offset$ 0Q�5      � 5      $ #C2E8FF��6   $ 1Q�5      5      .��5      �  � RowDefinitions 5   
   �� 5      $
��0.3* �6   �� 5      $
��0.2* �6   �� 5!      $��0.25* �6   �� 5#      $��0.25* �6   .+�5'   
   � $��0��6   $��2��5(      .+�5)      (� $��Center=�6   $��Bottom=�5*      $��70��5+      $��70��5,      .��5-      � .+�5/      A� $��80��6   $��80��50      
����������#D7B9FF$��3��53      �� 55      $��R��6    $��30(�56       $
��Bold"�57       
����p��$��Center=�59       $��Center=�5:       5;      5<      5=      � 5@   
   $��0,85,0,0q�6   $��2��5A      .+�5B      �� $+��%Nhập giá trị của điện trở��6   $��Center=�5C      $��Center=�5D      $��20(�5E      $
��Bold"�5F      
���   �5G      5H      � 5K   
   $��1��6   $��	0,57,0,85q�5L      $��3��5M      .+�5N      �� -   5        	 ��Name$	 QuantityTextBox��5N      $	��200��5O      $��40��5P      $��18(�5Q      
�������
���   �$��Center=�5T      $��Center=�5U      5W      5X      � 5[   
   $��3��6   .+�5\      �� -   5        $
��Save��5\      $	��100��5]      $��40��5^      ���#1976D2
�������$��16(�5a      $��Center=�5b      $	��Top=�5c      5d      '  !MaterialDesignThemes.Wpf.Snackbar  5e      -   5        
  Name$
 ErrorSnackbar��5e   &   $��Center=�5f   &   $��Bottom=�5g   &      MessageQueue 5h   &   �� *SnackbarMessageQueue+5i      5l      5m      