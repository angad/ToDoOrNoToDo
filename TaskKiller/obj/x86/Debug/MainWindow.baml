   M S B A M L   `   `   `  ����G  CToDoOrNoToDo, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:ViewView  5   	       View.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	    lclr-namespace:View     5   	   -   5          � Title$  
MainWindow��5   &   $	��480��69   $	��360��6F    � 
ResizeMode$ CanMinimize=�6R    � AllowsTransparency .  � WindowState$ Normal=�5   	    � WindowStyle$
 None=�6    � Icon$, &/ToDoOrNoToDo;component/image/icon.ico��6H    J� OpacityMask 61   Z� ��5	      �� 5
   
   .��5      � $	��489��6   $	��348��6    � ColumnDefinitions 5   
   �� 5      $
��502* �6   �� 5      $��* �6   .+�5   
   � -   5        $��cmdline��5   I   $��23��6   $
��Left=�6   $��	36,72,0,0q�66   $	��Top=�6X   $	��265��6p   ��5      �� 5       �� ImageSource$+ %/ToDoOrNoToDo;component/image/cmd.png��6   5      �� 5   
   -   5        $��commandLine��5      $��61,75,62,377q�5      
�������$��Courier New)�5      ��5      Z� ��5      Z� .��5   +   5      �� 5    
   -   5        $��status��5    _   	 �� Padding$	 20,20, 0, 0q�6   $
��Wrap=�6*   $	��119��6>   $��
0,357,35,0q�6K   $	��Top=�6m   
���   �$��Courier New)�6�   $��Right=�6�   $	��287��6�   ��5!      �� 5"      $. (/ToDoOrNoToDo;component/image/status.png��6   5$      � 5%   
   -   5        $��TextUIElement��5%      $��0,0,0,0q�6$   .+�5&      �� -   5        $��terminal��5&   <   $	��242��6   $��34,0,46,141q�6'   $��Bottom=�6L   
���   �$��Courier New)�6�   
 �� AcceptsReturn
 .  �� 
CaretBrush
 �   � �� IsReadOnlyCaretVisible .  " �� VerticalScrollBarVisibility$ Visible=�6�   $��12(�6  |�. ��6g   Z� ��65  Z�  � 	Resources 5'       �� 
TargetType �� Property �� Value��%X   'J�       �� 5)      " J�6   .�5*      ��  �@��Margin$ 0q�63   5+      5,      5-      5.      �� 5/   
   -   5        $��manbox��5/   5   $	��234��6   $
��Wrap=�6!   $��44,106,60,0q�6C   $	��Top=�6X   $��Courier New)�6�   ��6p   Z� 6�   � 50   
   -   5        $��
GuiElement��50      $��2��6!   $��0,0,0,0q�65   .+�51      &   System.Windows.Controls.DataGrid  -	   5        $��taskListDataGrid��54      $	��242��51         AutoGenerateColumns .  $��
0,106,48,0q�55      $	��Top=�56         CanUserResizeColumns .     CanUserReorderColumns .     GridLinesVisibility$
 None=�59         HorizontalGridLinesBrush �	#CCCCCCCC   VerticalGridLinesBrush �	#CCCCCCCC
���   �   CanUserResizeRows .     SelectionMode$ Single=�5?         SelectionUnit$ FullRow=�5@         CanUserAddRows .     ColumnHeaderHeight$ 20Q�5B      $��Right=�5D      $	��266��5E      $��Buxton Sketch)�5G      $��15(�5H      $��Normal"�6%   ��53      �� *TaskListView+��5<      Z� ��5C      Z�    RowBackground 5F      Z�    ColumnHeaderStyle 5I      �� 5J      =  7System.Windows.Controls.Primitives.DataGridColumnHeader"  6   .�5K      ��  � �� 5L      �� 5M   "   $0 */ToDoOrNoToDo;component/image/brushbox.png��6-   5P      �� 5Q       � ��
 �   �67   �� 5R       � ��$ 15(�65   �� 5S       � ��$ Buxton Sketch)�67   5T         RowHeaderStyle 5W      �� 5X      :  4System.Windows.Controls.Primitives.DataGridRowHeader"  6   .�5Y      ��  � �� 5Z      �� 5[   "   $0 */ToDoOrNoToDo;component/image/brushbox.png��6-   5]      �� 5^       � ��
 �   �67   �� 5_       � ��$ 13(�65   5`          Columns  5c      0  *System.Windows.Controls.DataGridTextColumn  5d      ,  &System.Windows.Controls.DataGridColumn!   Header$! ID��6)   "   Width5  /System.Windows.Controls.DataGridLengthConverter$" 25 65   #   
IsReadOnly# . 1  +System.Windows.Controls.DataGridBoundColumn$   Binding$ 6@   �� *ID+6k   4  .System.Windows.Controls.DataGridCheckBoxColumn  5e      $!  !��6-   $" 20 69   $ 6D   �� *Star+%   ElementStyle% 5f      �� 5g      " d�6$   .�5h   "   ��  � �� 5i   &   �� 5j   *   & �� 
TargetType"& d�6:   .R�5k   .   �� ' ��Name$' innerBorder��65   ��5l   2   �� 5m   6   $/ )/ToDoOrNoToDo;component/image/un-star.png��6A   ( j� Stretch$
( Fill=�6y   ) j� TileMode$
) None=�6�   5o   /   * �� Triggers* 5p   .   P� 5q   2   + P� Property+ �@d�	IsChecked, P� Value$
, TrueY�6O   . �5r   6   �� - �� 
TargetName$- innerBorder��6=    � �� 5s   :   �� 5t   >   $- '/ToDoOrNoToDo;component/image/s-tar.png��6I   5v   7   5w   3   5y   +   5{   #   5|      .   	CellStyle. 5~      �� 5      .�5�   "   -
   5        5�      5�        5�      $
! Name��6)   $" * 67   $ 6A   �� *Name+6\     5�      $! Deadline��6)   $" 60 6;   $ 6F   �� *
Deadline+6e   4	  .System.Windows.Controls.DataGridTemplateColumn	  5�      $! X��6-   / 	  CellTemplate/ 5�      �� 5�      .R�5�   "   �� -   5        $��17��5�   >   0 J� Opacity$0 0Q�6J   ��5�   &   �� 5�   *   $+ %/ToDoOrNoToDo;component/image/del.png��65   6k   5�   #   5�      5�      	  5�      $! +��6-   / 5�      �� 5�      .R�5�   "   �� -   5        $��17��5�   <   $0 0Q�6H   ��5�   &   �� 5�   *   $, &/ToDoOrNoToDo;component/image/tick.png��65   6l   5�   #   5�      5�      1   AlternatingRowBackground1 5�      �� 5�      $. (/ToDoOrNoToDo;component/image/row_bg.png��6!   5�      5�      � 5�   
   -   5        $��	about_pic��5�   J   $	��220��6   $
��Left=�6   $��	55,77,0,0q�67   $y�Hidden=�6[   $	��Top=�6o   $	��220��6�   ��5�      �� 5�      $@ :/ToDoOrNoToDo;component/image/twinkletwinklelittlestar.png��6   5�      � 5�   
   -   5        $��
about_name��5�   J   $��29��6   $
��Left=�6   $��
55,303,0,0q�66   $y�Hidden=�6\   $	��Top=�6p   $	��226��6�   ��5�      �� 5�      $, &/ToDoOrNoToDo;component/image/ttls.png��6   5�      �� 5�   
   -   5        $��minimize��5�   d   $�� ��6   $0 0Q�6   $��36��6(   $
��Left=�64   $��310,0,0,344q�6O   $��Bottom=�6�   $��36��6�   �� 5�   
   -   5        $
��home��5�   b   $�� ��6   $0 0Q�6   $��35��6(   $
��Left=�64   $��	34,-3,0,0q�6O   $	��Top=�6�   $��81��6�   �� 5�   
   -   5        $��	donetasks��5�   d   $�� ��6   $0 0Q�6   $��38��6(   $
��Left=�64   $��307,303,0,0q�6O   $	��Top=�6�   $��43��6�   �� 5�   
   -   5        $��close��5�   c   $�� ��6   $0 0Q�6   $��32��6(   $
��Left=�64   $��
309,71,0,0q�6O   $	��Top=�6�   $��34��6�   �� 5�   
   -   5        $��about��5�   d   $�� ��6   $0 0Q�6   $��38��6(   $
��Left=�64   $��309,400,0,0q�6O   $	��Top=�6�   $��42��6�   �� 5�   
   -   5        $
��help��5�   d   $�� ��6   $0 0Q�6   $��37��6(   $
��Left=�64   $��310,366,0,0q�6O   $	��Top=�6�   $��34��6�   ��5�   
   �� 5�      5�      ��5�      �� 5�   
   $9 3/ToDoOrNoToDo;component/image/main_bg_plus_help.png��6   5�      