package View;

import java.awt.Color;
import java.awt.Dimension;
import java.awt.Font;

import javax.swing.JLabel;
import javax.swing.JPanel;
import javax.swing.JTextField;

public class SynchronisationCloud extends JPanel{
	private JTextField jtf = new JTextField("");
	private JTextField jtf2 = new JTextField("");
	
	public SynchronisationCloud() {
		 Font police = new Font("Arial", Font.BOLD, 14);		 
		 jtf.setFont(police);
		 jtf.setPreferredSize(new Dimension(450, 30));
		 jtf.setForeground(Color.BLUE);
		 JLabel label = new JLabel("Email Dropbox");
		 this.add(label);
		 this.add(jtf);		 
		 jtf2.setFont(police);
		 jtf2.setPreferredSize(new Dimension(450, 30));
		 jtf2.setForeground(Color.BLUE);
		 JLabel label2 = new JLabel("Mot de passe");
		 this.add(label2);
		 this.add(jtf2);
	  }
	
	public void getText() {
		System.out.println("Email : "+ jtf.getText() + " mot de passe : "+ jtf2.getText());
	}
}
