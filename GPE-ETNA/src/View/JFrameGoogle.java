package View;

import java.awt.BorderLayout;
import java.awt.Desktop;
import java.awt.Dimension;
import java.awt.GridLayout;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.awt.event.KeyEvent;
import java.io.IOException;
import java.net.URI;
import java.net.URISyntaxException;

import javax.swing.ImageIcon;
import javax.swing.JButton;
import javax.swing.JComponent;
import javax.swing.JFrame;
import javax.swing.JLabel;
import javax.swing.JPanel;
import javax.swing.JTabbedPane;

import Controller.GoogleController;

//La fenetre de google
public class JFrameGoogle extends GoogleView implements ActionListener{

	private JFrame frame = null;
	String[] labels = { "First Name", "Middle Initial", "Last Name", "Age" };
    char[] mnemonics = { 'F', 'M', 'L', 'A' };
    int[] widths = { 15, 1, 15, 3 };
    String[] descs = { "First Name", "Middle Initial", "Last Name", "Age" };

	
	
	public JFrameGoogle(GoogleController controller){
		super(controller); 
 
		buildFrame();
	}
	
	private void buildFrame() {
		frame = new JFrame("test mvc page");
		
		frame.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
	   
		JTabbedPane tabbedPane = new JTabbedPane(JTabbedPane.BOTTOM);
		ImageIcon icon = new ImageIcon("https://media.giphy.com/media/l2R0aKwejYr8ycKAg/giphy.gif");

		final SynchronisationCloud sc =  new SynchronisationCloud(new GridLayout(5,2,20,50));
		JButton button = new JButton("Se connecter");
		button.addActionListener(new ActionListener() {			
			public void actionPerformed(ActionEvent e) {
				//se connecter sur l'url de scynchro				
				if(Desktop.isDesktopSupported())
				{
				    try {
				    	Desktop.getDesktop().browse(new URI("http://localhost:8080/API/listeFile"));
					} catch (IOException e1) {
						// TODO Auto-generated catch block
						e1.printStackTrace();
					} catch (URISyntaxException e1) {
						// TODO Auto-generated catch block
						e1.printStackTrace();
					}
				}				
							    
				sc.getText();
			}
		});
		sc.add(button);
		JComponent panel1 = sc;
		tabbedPane.addTab("Drive", icon, panel1, "Does nothing");
		tabbedPane.setMnemonicAt(0, KeyEvent.VK_1);

		JComponent panel2 = new ShowAllFiles();  //makeTextPanel("Panel #2");
		tabbedPane.addTab("File", icon, panel2, "Does twice as much nothing");
		tabbedPane.setMnemonicAt(1, KeyEvent.VK_2);

		JComponent panel3 = makeTextPanel("Panel #3");
		tabbedPane.addTab("Tab 3", icon, panel3,
		                  "Still does nothing");
		tabbedPane.setMnemonicAt(2, KeyEvent.VK_3);

		JComponent panel4 = makeTextPanel(
		        "Panel #4 (has a preferred size of 410 x 50).");
		panel4.setPreferredSize(new Dimension(410, 50));
		tabbedPane.addTab("Tab 4", icon, panel4,
		                      "Does nothing at all");
		tabbedPane.setMnemonicAt(3, KeyEvent.VK_4);
		
	    frame.add(tabbedPane, BorderLayout.CENTER);
	    frame.setSize(900, 600);
	    frame.setVisible(true);
	    
	}

	 protected JComponent makeTextPanel(String text) {
	        JPanel panel = new JPanel(false);
	        JLabel filler = new JLabel(text);
	        filler.setHorizontalAlignment(JLabel.CENTER);
	        panel.setLayout(new GridLayout(1, 1));
	        panel.add(filler);
	        return panel;
	    }
	        
	
	public void actionPerformed(ActionEvent e) {
		// TODO Auto-generated method stub
		System.out.println("Action Perf global");
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
