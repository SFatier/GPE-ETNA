package View;

import java.awt.GridLayout;

import javax.swing.JButton;
import javax.swing.JLabel;
import javax.swing.JPanel;
import javax.swing.JTextPane;

import fr.gpe.object.AccountDrive;

public class ShowTodoList extends  JPanel{
		private JTextPane textpan1 =  new JTextPane();
		private JTextPane textpan2 =  new JTextPane();
		private JTextPane textpan3 =  new JTextPane();
		private JTextPane textpan4 =  new JTextPane();
		
		/**
		 * Constructeur TodoList
		 * */
		public ShowTodoList(AccountDrive u) {
			if( u != null) {	
				setLayout(new GridLayout(2,2));
				
				 JLabel label_afaire = new JLabel("A Faire");
				 JPanel jp1 = new JPanel();				 
				 jp1.add(label_afaire);
				 textpan1.setSize(300, 300);
				 textpan1.setLocation(30, 100);
				 jp1.add(textpan1);
				 add(jp1);
				 
				 textpan2.setSize(300, 300);
				 add(textpan2);
				 textpan3.setSize(300, 300);
				 add(textpan3);
				 textpan4.setSize(300, 300);;
				 add(textpan4);
				    
			}else {
				System.out.println("Pas d'utilisateur");
			}
		}
}
