   M S B A M L   `   `   `  ����E  ATicketCine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'clr-namespace:TicketCine
TicketCine  5   	       TicketCine.Usuario   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   *localclr-namespace:TicketCine     5   	     � Title$  Usuario��5   	   $	��450��6   $	��800��6&   .��5	      � $��10q�6    � RowDefinitions 5
   
   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $��30 �6    � ColumnDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $��* �6   .+�5   
   �� $��Nombre:��6   $��0��6"   $��0��6/   �� 5   
   -   5         ��Name$ 	txtNombre��5      $��0��6%   $��1��62   $��5q�6B   �� 5   
   $��Género:��6   $��1��6"   $��0��6/   �� 5   
   -   5         ��Name$ 	cmbGenero��5      $��1��6&   $��1��63   $��5q�6C   ��5      �� *GenderOptions+�� 5    
   $��Edad:��6   $��2��6    $��0��6-   �� 5!   
   -   5        $ txtEdad��5!      $��2��6#   $��1��60   $��5q�6@   �� 5#   
   $��Correo:��6   $��3��6"   $��0��6/   �� 5$   
   -   5        $ 	txtCorreo��5$      $��3��6%   $��1��62   $��5q�6B   �� 5&   
   $��
Teléfono:��6   $��4��6$   $��0��61   �� 5'   
   -   5        $ txtTelefono��5'      $��4��6'   $��1��64   $��5q�6D   �� 5)   
   -   5        $��Guardar Cliente��5)      $��5��6+   $��1��68   6H   �� 5+   
   -   5         ��Name$ chkAgregarTarjetaPlus��5+      $��Agregar Tarjeta Plus��62   $��6��6Q   $��1��6^   $��5q�6n   5,      5-      