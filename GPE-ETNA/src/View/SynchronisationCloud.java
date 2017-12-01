package View;

import java.awt.Color;
import java.awt.Dimension;
import java.awt.Font;
import java.awt.GridLayout;

import javax.swing.JLabel;
import javax.swing.JPanel;
import javax.swing.JTextField;
import javax.swing.border.TitledBorder;

import fr.gpe.object.Utilisateur;

public class SynchronisationCloud extends JPanel{
	private JTextField jtf = new JTextField("");
	private JTextField jtf2 = new JTextField("");
	
	public SynchronisationCloud(GridLayout gridLayout, Utilisateur u) {
		Font police = new Font("Arial", Font.BOLD, 14);		 
		
		TitledBorder border = new TitledBorder("Synchonisation Dropbox");
		border.setTitleJustification(TitledBorder.LEFT);
		border.setTitlePosition(TitledBorder.TOP);
		
		this.setBorder(border);

		//Label email 
		JLabel label_email = new JLabel("Email :");
		label_email.setLocation(40, 70);
		label_email.setSize(128, 128);
		//Textbox email
		jtf.setFont(police);
		jtf.setLocation(40, 80);
		jtf.setPreferredSize(new Dimension(450, 30));
		jtf.setForeground(Color.black);
		jtf.setBounds(100,100,200,40);
				
		//Label Mdp
		JLabel label_mdp = new JLabel("Mot de passe :");
		label_mdp.setLocation(40, 50);
		label_mdp.setSize(128, 128);
		//Textbox mdp
		jtf2.setFont(police);
		jtf2.setPreferredSize(new Dimension(450, 30));
		jtf2.setForeground(Color.black);
		jtf2.setLocation(40, 100);
		jtf2.setSize(100, 80); 
		add(label_email);
		add(jtf);
		add(label_mdp);
		add(jtf2);
	  }
	
	public void getText() {
		System.out.println("Email : "+ jtf.getText() + " mot de passe : "+ jtf2.getText());
	}
}
