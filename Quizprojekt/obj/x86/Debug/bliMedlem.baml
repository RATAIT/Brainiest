   M S B A M L   `   `   `  ����  Quizprojekt    Quizprojekt.bliMedlem   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   $	��500��6@   $	��700��6M   $	��500��6Y   $	��700��6i   .��5      � -   5        $��gridBliMedlem��5        � 	Resources  5   
      lblAni_kommaIn g� FillBehavior �� TargetProperty t� From	 t� To g� Duration  lblAni_akaUt  lblAni_skaka��%p  &	        &	 �     &	 H    �� 5      .��5      t� -   5        $
 Stop=�5   %   $ Margin�6^   $ 750, 0, 0, 0X�6�   $ 445, 0, 0, 0X�6�   $ 0:0:0.4D�6�   6�   5      �� 5
      .��5      t� -   5        $
 Stop=�5   %   $ Margin�6\   $ 445, 0, 0, 0X�6   $ 750, 0, 0, 0X�6�   $ 0:0:0.4D�6�   6�   5      �� 5      .��5      t� $
 Stop=�6%   $ Margin�69   $ 425, 0, 0, 0X�6\   $ 445, 0, 0, 0X�6p   $ 0:0:0.1D�6�   6�   t� 5      $
 Stop=�6%   $ Margin�69   $ 445, 0, 0, 0X�6\   $ 425, 0, 0, 0X�6p   $ 0:0:0.1D�6�   6�   5      5       � RowDefinitions 5   
   �� 5      $��* �6   �� 5      $��40 �6   �� 5      $��40 �6   �� 5      $��40 �6   �� 5      $��40 �6   �� 5      $��* �6   .+�5   
   �� $��1��6   $��	200,7,0,0q�6   $��Användarnamn:��60   
�������$��
Swis721 BT)�6[   $��DemiBold"�6s   6�   �� 5!   
   -   5        $��lbl_AnvFinns��5"   I   $��1��5!      $��Center=�6   $��445, 0, 0, 0q�6:   $��Center=�6P   $!��Användarnamnet finns redan��5"      $y�Hidden=�65   ���	#FFFF41416v   �� 5$   
   -   5        $��
txtbox_Anv��5&   &   $��1��5$      $�� ��6   
�������$��Center=�6:   $��Center=�5%      $	��165��64   $��25��6@   $��Center=�6L   ���	#FF6A6A6A$��0q�5&      $��	100,0,0,0q�68   .��6l    J� Effect 5'      3  -System.Windows.Media.Effects.DropShadowEffect  5(         ShadowDepth$ 5Q�6#   	   Color$	 Black��63   
   Opacity$
 .4Q�6A      RenderingBias$ Quality=�6N      	Direction$	 -70Q�6f   .��5)      5*      �� 5-   
   $��2��6   $��	200,7,0,0q�6   $��
Lösenord:��61   
�������$��
Swis721 BT)�6Y   $��DemiBold"�6q   6�   G� 5.   
   -   5        $��txtbox_Password��5.   '    G� PasswordChar$ *��6   $��25��6>   
�������$��Center=�6]   $��Center=�5/      $��2��68   $��1��6E   $	��165��6U   $��Center=�6a   ���	#FF6A6A6A$��0q�50   -    G� Password$  ��6A   $��	100,0,0,0q�6M    53        54      $ 5Q�6#   $	 Black��63   $
 .4Q�6A   $ Quality=�6N   $	 -70Q�6f   56      �� 57   
   -   5        $��lbl_MatcharEj��58   E   $��3��57      $��Center=�6   $��425, 0, 0, 0q�6:   $��Center=�6P   $��Lösenorden matchar ej.��58      $y�Hidden=�61   ���	#FFFF41416s   �� 5;   
   $��3��6   $��	200,7,0,0q�6   $��Lösenord igen:��61   
�������$��
Swis721 BT)�6]   $��DemiBold"�6u   6�   G� 5<   
   -   5        $��txtbox_Password2��5<   '   $ *��6   $��25��6?   
�������$��Center=�6^   $��Center=�5=      $��3��68   $��1��6E   $	��165��6U   $��Center=�6a   ���	#FF6A6A6A$��0q�5>   -   $  ��6A   $��	100,0,0,0q�6M    5A        5B      $ 5Q�6#   $	 Black��63   $
 .4Q�6A   $ Quality=�6N   $	 -70Q�6f   5D      �� 5H   
   -	   5        $��btn_BliMedlem��5I   U   $��4��5H      $��1��6   $��80��6.   $��
Bli Medlem��69   $��30��6N   $��0,0,85,0q�6Z   
�������$��
Swis721 BT)�5I      $��0q�6)   ���	#00000000$y�Hidden=�6j   ��5L      �� 5M       �� EndPoint$ 0.5,1&�6&    �� 
StartPoint$ 0.5,0&�67   .��5N      �  � Color$ 	#FF5EF677��6#    � Offset$ 0Q�65   � 5O      $ 	#FF1E7C30��6#   $ 1Q�65   5P      5S      �� 5V   
   -
   5        $��
btn_Avbryt��5W   g   $��4��5V      $��1��6   $��80��6.   $��Tillbaka��69   $��30��6L   $��85,0,0,0q�6X   
�������|�.  $��
Swis721 BT)�5W       J� 	AllowDrop .  $��0q�6;   ���	#00000000��5Z      �� 5[      $ 0.5,1&�6&   $ 0.5,0&�67   .��5\      � $ 	#FF5EF677��6#   $ 0Q�65   � 5]      $ 	#FF1E7C30��6#   $ 1Q�65   5^      5a      5c      5d      