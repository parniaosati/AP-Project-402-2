   M S B A M L   `   `   `  ���� 
  Login1    Login1.ItemWindow   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml       � Title$  
ItemWindow��$	��800��$	��600�� � 	Resources     BooleanToVisibilityConverter��%   &	        �� .���� $
��Auto=�.���� $��10q�.+��� $��Restaurant Menu��$��24(�$��0,0,0,20q�$��Center=��� -    ��Name$ 	ItemImage��$	��200��$	��200��$��0,0,0,20q��� -    ��Name$ ItemName��$��18(�$
��Bold"��� -   $ ItemDescription��$��0,0,0,20q��� -   $ 	ItemPrice���� -   $ ItemQuantity��$��0,0,0,20q��� -   $ AverageRating��$��16(�$
��Bold"�$��0,0,0,20q��� $��Rate this Item��$��16(�$
��Bold"��� -    ��Name$ RatingComboBox��$	��100��$��0,0,0,10q�.@��� $��1���� $��2���� $��3���� $��4���� $��5���� -   $��Submit Rating��$��0,0,0,20q�{��� *IsRatingEnabled+�� $��Leave a Comment��$��16(�$
��Bold"��� -	    ��Name$ CommentTextBox��$	��100��$��0,0,0,10q��� -
   $��Submit Comment��$��0,0,0,20q��� $��Comments��$��16(�$
��Bold"��� -    ��Name$ CommentsList��$	��300��$��0,0,0,20q����� *
Comments+ �� View �� .P��� $
��User�� �� Width$	 100��	 �� DisplayMemberBinding	 �� *
UserName+�� $��Comment��$	 200��	 �� *CommentText+�� $��Rating��$ 50��	 �� *Rating+�� $
��Time��$	 150��	 �� *CommentTime+�� $��Actions��$	 150��
 �� CellTemplate
 �� .R���  �� Orientation$ 
Horizontal=�.+��� -   $
��Edit��$��0,0,5,0q�y��� *CanEditOrDelete+ �� 	Converter# [  �� -   $��Delete��y��� *CanEditOrDelete+# [  �� -   $��Add to Cart��