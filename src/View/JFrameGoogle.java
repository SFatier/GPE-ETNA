package View;

import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;

import javax.swing.JButton;
import javax.swing.JFrame;
import javax.swing.JPanel;

import Controller.GoogleController;

//La fenetre de google
public class JFrameGoogle extends GoogleView implements ActionListener{

	private JFrame frame = null;
	private JPanel contentPane = null;
 	private JButton button = null;
 	
	public JFrameGoogle(GoogleController controller){
		super(controller); 
 
		buildFrame();
	}
	
	private void buildFrame() {
		frame = new JFrame("test mvc page");		 
		contentPane = new JPanel();
 
		button = new JButton("Mettre à jour");
		button.addActionListener(this);
		contentPane.add(button);
 
		frame.add(contentPane);
		frame.setSize(300, 300);
		//frame.setLocation(null);
		frame.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
		frame.setVisible(true);
	}

	@Override
	public void actionPerformed(ActionEvent e) {
		// TODO Auto-generated method stub
		System.out.println("click sur le bouton");
	}

	@Override
	public void display() {
		frame.setVisible(true);		
	}

	@Override
	public void close() {
		frame.dispose();		
	}
}
