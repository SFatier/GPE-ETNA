package View;

import java.awt.Color;
import java.awt.Dimension;
import java.awt.Font;
import java.awt.GridBagConstraints;
import java.awt.GridBagLayout;
import java.awt.GridLayout;

import javax.swing.JButton;
import javax.swing.JLabel;
import javax.swing.JPanel;
import javax.swing.JTextField;

public class SynchronisationCloud extends JPanel{
	private JTextField jtf = new JTextField("");
	private JTextField jtf2 = new JTextField("");
	
	public SynchronisationCloud(GridLayout gridLayout) {
		JLabel label = new JLabel("Email :");
    	this.add(label);
    	label.setLocation(40, 20);
    	label.setSize(128, 128);
		 
		 Font police = new Font("Arial", Font.BOLD, 14);		 
		 jtf.setFont(police);
		 jtf.setPreferredSize(new Dimension(450, 30));
		 jtf.setForeground(Color.black);
		 jtf.setBounds(100,100,200,40);
		
		 this.add(jtf);
		 
		 JLabel label2 = new JLabel("Email :");
	     this.add(label2);
	     label2.setLocation(40, 70);
	     label2.setSize(128, 128);
		 
		 jtf2.setFont(police);
		 jtf2.setPreferredSize(new Dimension(450, 30));
		 jtf2.setForeground(Color.black);
		 jtf2.setLocation(50, 80);
		 jtf2.setSize(100, 80);	 
		 
		 this.add(jtf2);		  		
	  }
	
	public void getText() {
		System.out.println("Email : "+ jtf.getText() + " mot de passe : "+ jtf2.getText());
	}
}
