   M S B A M L   `   `   `  ����G  CCheckersGame, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null+clr-namespace:CheckersGameCheckersGame  5   	   -    'CheckersGame.PlayerIdentificationWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   ,localclr-namespace:CheckersGame     5   	     � Title$  Player Identification��5   	   $	��170��6'   $	��300��64    � 
ResizeMode$ NoResize=�6@    � WindowStartupLocation$ CenterScreen=�6V    � Icon$ Resources/Icon.ico��6{    J� Effect 5	      3  -System.Windows.Media.Effects.DropShadowEffect  5
   
   .��5      � .+�5   
   �� -   5         ��Name$ textBox2��5      -  'System.Windows.Input.KeyboardNavigation   TabIndex$ 1��6$   $
��Left=�6D   $��23��6_   $��
120,65,0,0q�6k    �� TextWrapping$
 Wrap=�6   $	��Top=�6�   $	��162��6�   �� 5   
   -   5         ��Name$ label��5      $��Enter player1 name:��6   $
��Left=�6=   $��0,36,0,0q�6X   $	��Top=�6j   �� 5   
   -   5        $ textBox1��5      $ 0��6$   $
��Left=�6D   $��22��6_   $��
120,40,0,0q�6k   $
 Wrap=�6   $	��Top=�6�   $	��162��6�   �� 5   
   -   5        $ 
label_Copy��5      $��Enter player2 name:��6$   $
��Left=�6B   $��0,62,0,0q�6]   $	��Top=�6o   �� 5   
   -   5        	 ��Name$	 button��5      $ 2��6!   $��OK��6A   $
��Left=�6N   $��109,108,0,0q�6i   $	��Top=�6~   $��75��6�   6�   5      5      