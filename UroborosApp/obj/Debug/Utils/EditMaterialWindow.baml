   M S B A M L   `   `   `  ����  UroborosApp*    $UroborosApp.Utils.EditMaterialWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$1  +Редактировать материал��5   	   $	��400��6(   $	��400��65   .��5      � $��10q�6    � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5	      $
��Auto �6   �� 5
      $
��Auto �6   �� 5      $
��Auto �6   .+�5   
   �� $��0��6   $��Название:��6!   �� 5   
   $��1��6   $��0,5,0,0q�6[   ��6   �� *title+ �� UpdateSourceTrigger$ PropertyChanged=�6[   �� 5   
   $��2��6   $��Содержимое:��6!   �� 5   
   $��3��6    �� AcceptsReturn .  �� TextWrapping$
 Wrap=�6r   $	��100��6�   $��0,5,0,0q�6�   ��6   �� *	content+$ PropertyChanged=�6�   �� 5   
   $��4��6    �� Orientation$ 
Horizontal=�6"   $��Right=�6;   $��0,10,0,0q�6W   .+�5      �� -   5        $��Сохранить��5      6)   �� 5      -   5        $��Отмена��5       �� IsCancel . 6;   5      5      5      