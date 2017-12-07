package View;

import java.awt.Color;
import java.awt.Desktop;
import java.awt.Dimension;
import java.awt.Font;
import java.awt.GridLayout;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.io.IOException;
import java.net.URI;
import java.net.URISyntaxException;

import javax.swing.JButton;
import javax.swing.JComponent;
import javax.swing.JLabel;
import javax.swing.JPanel;
import javax.swing.JTextField;
import javax.swing.border.TitledBorder;

import fr.etnagpe.dropbox.DropBoxConnexion;
import fr.gpe.object.Utilisateur;

public class SynchronisationCloud extends JPanel{
	private String uri_auth = DropBoxConnexion.SendURLAuth();
	private JTextField jtf = new JTextField("");
	private JTextField jtf2 = new JTextField("");
	private JTextField jtf_email_google = new JTextField("");
	private JTextField jtf_mdp_google = new JTextField("");
	private Utilisateur u = new Utilisateur();
	Font police = new Font("Arial", Font.BOLD, 14);	 
	
	public SynchronisationCloud(GridLayout gridLayout, Utilisateur _u) {
		u = _u;		
		
		//Panel1 - Dropbox
		JPanel p = BorderDropbox();
		
		//Panel 2 - Google
		JPanel p2 = BorderGoogle();
		
		//Ajout des deux panels dans le Courant Panel
		this.add(p);
		this.add(p2);
		this.setBackground(Color.white);
	  }
	
	private JPanel BorderGoogle() {
		JPanel p2 = new JPanel();
		p2.setLocation(0, 70);
		p2.setSize(600, 128);
		
		//Encadrement Google
		TitledBorder border_google = new TitledBorder("Synchonisation Google Drive");
		border_google.setTitleJustification(TitledBorder.LEFT);
		p2.setBorder(border_google);
				
		//Label email 
		JLabel label_email_google = new JLabel("Email :");
		label_email_google.setLocation(40, 70);
		label_email_google.setSize(128, 128);
		//Textbox email
		jtf_email_google.setFont(police);
		jtf_email_google.setLocation(40, 80);
		jtf_email_google.setPreferredSize(new Dimension(150, 30));
		jtf_email_google.setForeground(Color.black);
		jtf_email_google.setBounds(100,100,200,40);
		
		//Label Mdp
		JLabel label_mdp_google = new JLabel("Mot de passe :");
		label_mdp_google.setLocation(40, 50);
		label_mdp_google.setSize(128, 128);
		//Textbox mdp
		jtf_mdp_google.setFont(police);
		jtf_mdp_google.setPreferredSize(new Dimension(150, 30));
		jtf_mdp_google.setForeground(Color.black);
		jtf_mdp_google.setLocation(40, 100);
		jtf_mdp_google.setSize(100, 80); 
		
		p2.add(label_email_google);
		p2.add(jtf_email_google);
		p2.add(label_mdp_google);
		p2.add(jtf_mdp_google);
		
		JButton button_google = new JButton("Se connecter");
		button_google.addActionListener(new ActionListener() {			
			public void actionPerformed(ActionEvent e) {				
				System.out.print("Non implémenté.");
				// Auth(u);
				//getText();
			}		
		});
		p2.add(button_google);		
		return p2;
	}

	private JPanel BorderDropbox() {
		JPanel p = new JPanel();
		p.setSize(600, 200);
		TitledBorder border_dropbox = new TitledBorder("Synchonisation Dropbox");
		border_dropbox.setTitleJustification(TitledBorder.LEFT);
		p.setBorder(border_dropbox);

		//Label email 
		JLabel label_email = new JLabel("Email :");
		label_email.setLocation(40, 70);
		label_email.setSize(128, 128);
		//Textbox email
		jtf.setFont(police);
		jtf.setLocation(40, 80);
		jtf.setPreferredSize(new Dimension(150, 30));
		jtf.setForeground(Color.black);
		jtf.setBounds(100,100,200,40);
				
		//Label Mdp
		JLabel label_mdp = new JLabel("Mot de passe :");
		label_mdp.setLocation(40, 50);
		label_mdp.setSize(128, 128);
		//Textbox mdp
		jtf2.setFont(police);
		jtf2.setPreferredSize(new Dimension(150, 30));
		jtf2.setForeground(Color.black);
		jtf2.setLocation(40, 100);
		jtf2.setSize(100, 80); 
		p.add(label_email);
		p.add(jtf);
		p.add(label_mdp);
		p.add(jtf2);
		
		JButton button_dropbox = new JButton("Se connecter");
		button_dropbox.addActionListener(new ActionListener() {			
			public void actionPerformed(ActionEvent e) {
				   Auth(u);				    
				   getText();
			}		
		});
		p.add(button_dropbox);
		return p;
	}

	/**
	 * Ouvre le navigateur pour autoriser l'application a accéder aux différents documents
	 * */
	private void Auth(Utilisateur u) {				
		if(Desktop.isDesktopSupported())
		{
			 try {				    	
			    Desktop.getDesktop().browse(new URI(uri_auth));
			    if (u.token == null) {
			    	u.token = DropBoxConnexion.GetToken();
			    		}
			    System.out.println("Token btn : " + u.token);
			 } catch (IOException e1) {
				 e1.printStackTrace();
			 } catch (URISyntaxException e1) {
				 e1.printStackTrace();
			 }
		}
	}
	
	/**
	 * Renvoie le texte et le mot de passe de l'utilisateur
	 * **/
	public void getText() {
		System.out.println("Email : "+ jtf.getText() + " mot de passe : "+ jtf2.getText());
	}
}
