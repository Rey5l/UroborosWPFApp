   M S B A M L   `   `   `  ����  UroborosApp+    %UroborosApp.Utils.LearningGoalsDialog   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	     � Title$+  %Редактирование цели��5   	   $	��350��5   	   $	��400��5	   	    � WindowStartupLocation$ CenterOwner=�5
   	    � 
ResizeMode$ NoResize=�5   	   .��5      � $��10q�6    � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   .+�5   
   �� $��0��6   $��Название:��6!   $��14(�62   $��0,0,0,5q�6@   �� 5   
   -   5         ��Name$ TitleTextBox��5      $��1��6   $	��360��5      $��0,0,0,10q�5      ��5      �� *Title+ �� UpdateSourceTrigger$ PropertyChanged=�5      �� 5   
   $��2��6   $��Описание:��6!   $��14(�62   $��0,0,0,5q�6@   �� 5   
   -   5        $ DescriptionTextBox��5      $��3��6   $	��360��5       $��40��5!       �� AcceptsReturn .  �� TextWrapping$
 Wrap=�5#      $��0,0,0,10q�5$      ��5      �� *Description+$ PropertyChanged=�5$      �� 5&   
   $��4��6   $$��Срок выполнения:��6!   $��14(�69   $��0,0,0,5q�6G   (  "System.Windows.Controls.DatePicker  5'   
   -   5          Name$ TargetDatePicker��5'   "   $��5��6   $	��360��5)      $��0,0,0,20q�5*      	   SelectedDate	 5(      �� *Target_Date+$ PropertyChanged=�5*      �� 5,   
   $��6��6   
 �� Orientation$
 
Horizontal=�6"   $��Right=�6;   .+�5-      �� -   5        $��Сохранить��5-      $	��100��6)   65   �� 5.      -   5        $��Отмена��5.      $	��100��6&    �� IsCancel . 6B   5/      50      51      