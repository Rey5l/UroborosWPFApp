   M S B A M L   `   `   `  ����  UroborosApp)    #UroborosApp.Utils.RecallScoreDialog   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$+  %Оцените запоминание��5   	   $	��300��5   	    � SizeToContent$ Height=�5   	    � 
ResizeMode$ NoResize=�5   	    � WindowStartupLocation$ CenterOwner=�5   	   .��5	      �� $��15q�6   .+�5
   
   �� $K��EНасколько хорошо вы помните материал?��6   $
��Wrap=�5      $��0,0,0,10q�5      A� 5   
    A� Columns$ 5��6   $
��0,10q�6"   .+�5      	� -   5         	�Name$ Score1��5         RadioButtonStyle#��[  6*    	� 	GroupName$ RecallScore��6T   $��1��6l   
 � Tag$ 1��6x    � ToolTip$" Совсем не помню��5      	� 5      -   5        $ Score2��5      #��[  6*   $ RecallScore��6T   $��2��6l   $ 2��6x   $ 
Плохо��5      	� 5      -   5        $ Score3��5      #��[  6*   $ RecallScore��6T   $��3��6l   $ 3��6x   $ Нормально��5      	 d� 	IsChecked$
	 TrueY�6.   	� 5      -   5        $ Score4��5      #��[  6*   $ RecallScore��6T   $��4��6l   $ 4��6x   $ Хорошо��5      	� 5      -   5        $ Score5��5      #��[  6*   $ RecallScore��6T   $��5��6l   $ 5��6x   $ Отлично��5      5      �� 5   
   -   5        $��Подтвердить��5      $��Right=�5      $��0,15,0,0q�6,   5      5      