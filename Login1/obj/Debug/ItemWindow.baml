   M S B A M L   `   `   `  ����
  Login1    Login1.ItemWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$  
ItemWindow��5   	   $	��800��6   $	��600��6)   ���	#FFF8F1E1 � 	Resources 5          BooleanToVisibilityConverter��%   &	        �� 5   
   6'   5      .��5      �� $
��Auto=�6   .��5	   
   �� $��10q�6   .+�5
      �� $��Restaurant Menu��6   $��24(�6/   $��0,0,0,20q�6=   $��Center=�6O   ���#8B45136l   �� 5      ���	#FFE3A869 �� Padding$ 10q�6,   ���	#FF8B4513$��2q�6Q   $��0,0,0,20q�6e   ._�5      �� -   5         ��Name$ 	ItemImage��5      $	��200��6+   $	��200��67   5      �� 5      -   5         ��Name$ ItemName��5      $��18(�6*   $
��Bold"�68   ���#8B45136J   �� 5      -   5        $ ItemDescription��5      $��0,0,0,20q�61   ���#8B45136C   �� 5      -   5        $ 	ItemPrice��5      ���#8B45136+   �� 5      -   5        $ ItemQuantity��5      $��0,0,0,20q�6.   ���#8B45136@   �� 5      -   5        $ AverageRating��5      $��16(�6/   $
��Bold"�6=   $��0,0,0,20q�6O   ���#8B45136a   �� 5      $��Rate this Item��6   $��16(�6.   $
��Bold"�6<   ���#8B45136N   �� 5      -   5         ��Name$ RatingComboBox��5      $	��100��6/   $��0,0,0,10q�6;   .@�5      �� $��1��6   �� 5      $��2��6   �� 5      $��3��6   �� 5      $��4��6   �� 5      $��5��6   5      �� 5      -   5        $��Submit Rating��5      $��0,0,0,20q�6n   ���	#FFE3A869���#8B4513���	#FF8B4513{�6H   �� *IsRatingEnabled+6�   �� 5      $��Leave a Comment��6   $��16(�6/   $
��Bold"�6=   ���#8B45136O   �� 5      -	   5         ��Name$ CommentTextBox��5      $	��100��6.   $��0,0,0,10q�6;   ���	#FFF8F1E1���	#FF8B4513���#8B45136|   �� 5       -
   5        $��Submit Comment��5       $��0,0,0,20q�6J   ���	#FFE3A869���#8B4513���	#FF8B45136�   �� 5"      $��Comments��6   $��16(�6(   $
��Bold"�66   ���#8B45136H   �� 5#      -   5         ��Name$ CommentsList��5#      $	��300��6-   $��0,0,0,20q�6:   ���	#FFF8F1E1���	#FF8B4513��6L   �� *
Comments+ �� View 5$      �� 5%      .P�5&      �� $
��User��6)   	 �� Width$		 100��6a   
 �� DisplayMemberBinding
 67   �� *
UserName+6a   �� 5'      $��Comment��6)   $		 200��6g   
 6:   �� *CommentText+6g   �� 5(      $��Rating��6)   $	 50��6a   
 69   �� *Rating+6a   �� 5)      $
��Time��6)   $		 150��6d   
 67   �� *CommentTime+6d   �� 5*      $��Actions��6)   $		 150��6:    �� CellTemplate 5+      �� 5,   "   .R�5-   &   ��  �� Orientation$ 
Horizontal=�61   .+�5.   *   �� -   5        $
��Edit��5.   1   $��0,0,5,0q�6Z   ���	#FFE3A869���#8B4513���	#FF8B4513y�5/   1   �� *CanEditOrDelete+ �� 	Converter# [  �� 50   *   -   5        $��Delete��50   1   ���	#FFE3A869���#8B4513���	#FF8B4513y�51   1   �� *CanEditOrDelete+# [  52   '   53   #   55      56      58      �� 59      -   5        $��Add to Cart��59      ���	#FFE3A869���#8B4513���	#FF8B45136o   5:      5;      5<      