   M S B A M L   `   `   `  ����F  BUroborosApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null5clr-namespace:UroborosApp.PagesUroborosApp.Pages  5      5clr-namespace:UroborosApp.UtilsUroborosApp.Utils  5      *    $UroborosApp.Pages.ReviewSchedulePage   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      1localclr-namespace:UroborosApp.Pages     5      1utilsclr-namespace:UroborosApp.Utils     5        N� Title$  ReviewSchedulePage��5
       � 	Resources 5      ,   &UroborosApp.Utils.DateToTodayConverter   DateToTodayConverter��%   &	          5   
   6%   5      .��5      � 
�������.+�5   
   ]� $��Center=�6   $	��Top=�61   $
��Auto��6I   .+�5      ��  �� Orientation$ 
Horizontal=�6   $
��Left=�62   $��10q�6H   .+�5      �� $��Uroboros��6   $��23(�6,   $��Center=�6:   $
��Bold"�6U   5      �� 5      $ 
Horizontal=�6   $��Center=�62   $��Center=�6O   .+�5      �� -   5        $
��Home��5        NavButtonStyle#��[ 6(   6P   �� 5      -   5        $��Category��5      #��[ 6,   6T   �� 5      -   5        $��Reminder��5      #��[ 6,   6T   �� 5      $��ReviewSchedule��6     NavButtonActiveStyle#��[ 62   �� 5      -   5        $��Progress��5      #��[ 6K   �� 5      -   5        $��
Statistics��5      #��[ 6O   �� 5      -   5        $��Tasks��5      #��[ 6E   �� 5      -   5        $��ActivityLog��5      #��[ 6M     MainFont#��[ 6u   �� 5      -   5        $��LearningGoals��5      #��[ 6U   #��[ 6}   �� 5       -	   5        $��Profile��5       #��[ 6+   6S   5!      5"      � 5$   
   $��10,50,10,10q�6    � RowDefinitions 5%      �� 5&      $
��Auto �6    �� 5'      $��* �6    .+�5*      �� $��0��6   $ 
Horizontal=�6&   .+�5+      �� $'��!График повторений��6   $��18(�65   $
��Bold"�6C   $��Bottom=�6U   �� 5,      -
   5         ��Name$ FilterComboBox��5,      $	��150��63   $��
20,10,10,0q�6?   .@�5.      �� $!��Только сегодня��6#   �� 5/      $#��Ближайшие 7 дней��6#   �� 50      $��Просроченные��6#   51      �� 52      -   5        $��Обновить��52      $	��100��6F   $��10,0,0,0q�6R   53      &   System.Windows.Controls.DataGrid  55      -   5          Name$ ScheduleList��55      $��1��6-   $��0,20,0,0q�6:      AutoGenerateColumns .     
IsReadOnly .    CanUserAddRows .  	   Columns	 57      0  *System.Windows.Controls.DataGridTextColumn  58      ,  &System.Windows.Controls.DataGridColumn
   Header$
 Материал��6)      Width5  /System.Windows.Controls.DataGridLengthConverter$	 250 5:   )   1  +System.Windows.Controls.DataGridBoundColumn   Binding 59   )   �� *MaterialTitle+5:   )     5;      $-
 'Следующее повторение��6)   $	 150 5=   )    5<   )   �� *
DateTime+ �� StringFormat$ {0:dd.MM.yyyy HH:mm}��5=   )   4  .System.Windows.Controls.DataGridTemplateColumn  5>      $
 Статус��6-   $	 120 6=      CellTemplate 5?      �� 5@      .R�5A   "   �� ��5B   &   �� 5C   *    �� 
TargetType" ��60   .�5D   .   ��  �� Property � �� �� Value$  Запланировано��6E   �� 5E   .    � ��
 � d �6K    �� Triggers 5F   .   �� 5G   2    �� Binding 6>   �� *
DateTime+ �� 	Converter# [   �� Value True6�   .a�5H   6   ��  � ��$ Сегодня!��6M   �� 5I   6    � ��
 �  ��6S   �� 5J   6    � ��$
 Bold"�6S   5K   3   5M   +   5O   #   5P      5R        5S      $
 Действия��6-   $	 150 6?    5T      �� 5U      .R�5V   "   �� $ 
Horizontal=�6-   .+�5W   &   �� -   5        $	��✅��5W   -    � ToolTip$4 .Отметить как повторенное��69   5X   -   �� 5Y   &   -   5        $
��🔄��5Y   -   $- 'Перенести повторение��6:   5Z   -   5[   #   5\      5^      5`      5a      5b      5c      