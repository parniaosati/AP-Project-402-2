   M S B A M L   `   `   `  ����
  Login1$    Login1.SearchRestaurantsWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$  SearchRestaurantsWindow��5   	   $	��800��6)   $
��1000��66   .��5      � $��10q�6    � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $��* �6   .+�5   
   �� $��0��6   .+�5      �� $��Search Restaurants��6   $��24(�62   $��0,0,0,20q�6@   $��Center=�6R   �� 5       �� Orientation$ 
Horizontal=�6   $��	0,10,0,10q�62   .+�5      �� $��City:��6   $	��100��6)   �� 5      -   5         ��Name$ CityTextBox��5      $	��200��6/   5      �� 5      $ 
Horizontal=�6   $��	0,10,0,10q�62   .+�5      �� $��Type:��6   $	��100��6)   �� 5      -   5        $ TypeTextBox��5      $	��200��6/   5      �� 5      $ 
Horizontal=�6   $��	0,10,0,10q�62   .+�5      �� $��Rating:��6   $	��100��6+   �� 5      -   5        $ SearchRatingTextBox��5      $	��200��67   5      �� 5      -   5        $��Search��5      $	��120��6&   $��Center=�6M   $��20q�6j   5      �� 5   
   -   5         ��Name$ RestaurantList��5      $��20q�6+   $��1��67   $	��300��6D    �� View 5      �� 5      .P�5      �� $
��Name��6%    �� Width$	 150��6Y    �� DisplayMemberBinding 63   �� *Name+6Y   �� 5       $
��City��6%   $	 100��6Y    63   �� *City+6Y   �� 5!      $
��Type��6%   $	 100��6Y    63   �� *Type+6Y   �� 5"      $��Rating��6%   $	 100��6]    65   �� *Rating+6]   5#      5%      5&      5'      