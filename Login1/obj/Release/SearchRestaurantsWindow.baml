   M S B A M L   `   `   `  ���� 
  Login1$    Login1.SearchRestaurantsWindow   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml       � Title$  SearchRestaurantsWindow��$	��800��$
��1000��.��� $��10q� � RowDefinitions �� $
��Auto ��� $��* �.+��� $��0��.+��� $��Search Restaurants��$��24(�$��0,0,0,20q�$��Center=���  �� Orientation$ 
Horizontal=�$��	0,10,0,10q�.+��� $��City:��$	��100���� -    ��Name$ CityTextBox��$	��200���� $ 
Horizontal=�$��	0,10,0,10q�.+��� $��Type:��$	��100���� -   $ TypeTextBox��$	��200���� $ 
Horizontal=�$��	0,10,0,10q�.+��� $��Rating:��$	��100���� -   $ SearchRatingTextBox��$	��200���� -   $��Search��$	��120��$��Center=�$��20q��� -    ��Name$ RestaurantList��$��20q�$��1��$	��300�� �� View �� .P��� $
��Name�� �� Width$	 150�� �� DisplayMemberBinding �� *Name+�� $
��City��$	 100�� �� *City+�� $
��Type��$	 100�� �� *Type+�� $��Rating��$	 100�� �� *Rating+