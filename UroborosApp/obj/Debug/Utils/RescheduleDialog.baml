   M S B A M L   `   `   `  ����F  BUroborosApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null5clr-namespace:UroborosApp.UtilsUroborosApp.Utils  5   	   (    "UroborosApp.Utils.RescheduleDialog   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   4controlsclr-namespace:UroborosApp.Utils     5   	     � Title$-  'Перенести повторение��5   	   $	��300��5   	    � SizeToContent$ Height=�5   	    � 
ResizeMode$ NoResize=�5   	   .��5	      �� $��10q�6   .+�5
   
   �� $��Новая дата:��6   $��0,0,0,5q�6'   (  "System.Windows.Controls.DatePicker  5   
   -   5          Name$ 
DatePicker��5         SelectedDate 6)   �� *
Tomorrow+�� 5   
   $��Время:��6   $��0,10,0,5q�6"   )   #UroborosApp.Utils.TimePickerControl  5   
   -   5          Name$ 
TimePicker��5   %   $��0,0,0,10q�69   �� 5   
   -   5        $��Сохранить��5      $��Right=�68   5      5      