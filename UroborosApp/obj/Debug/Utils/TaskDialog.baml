   M S B A M L   `   `   `  ����  UroborosApp"    UroborosApp.Utils.TaskDialog   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	     � Title$/  )Редактирование задачи��5   	   $	��300��5   	   $	��400��5	   	    � WindowStartupLocation$ CenterOwner=�5
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
   �� $��0��6   $��Материал:��6!   $��14(�62   $��0,0,0,5q�6@   �� 5   
   -   5         ��Name$ MaterialComboBox��5       $��1��6    �� DisplayMemberPath$ title��5       �� SelectedValuePath$ id��5      $	��360��5      $��0,0,0,10q�5       �� SelectedValue 5      �� *material_id+5      �� 5   
   $��2��6   $��Вопрос:��6!   $��14(�60   $��0,0,0,5q�6>   �� 5    
   -   5         ��Name$ QuestionTextBox��5       $��3��6   $	��360��5"      $��40��5#      	 �� AcceptsReturn	 . 
 �� TextWrapping$

 Wrap=�5%      $��0,0,0,10q�5&      ��5!      �� *
question+5&      �� 5(   
   $��4��6   $��Ответ:��6!   $��14(�6/   $��0,0,0,5q�6=   �� 5)   
   -   5        $ AnswerTextBox��5)      $��5��6   $	��360��5+      $��40��5,      	 . $

 Wrap=�5.      $��0,0,0,20q�5/      ��5*      �� *answer+5/      �� 51   
   $��6��6    �� Orientation$ 
Horizontal=�6"   $��Right=�6;   .+�52      �� -   5        $��Сохранить��52      6)   �� 53      -   5        $��Отмена��53       �� IsCancel . 66   54      55      56      