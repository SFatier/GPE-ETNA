package View;

import java.awt.Dimension;
import java.awt.GridLayout;
import java.awt.Toolkit;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;

import javax.swing.JFrame;
import javax.swing.JMenu;
import javax.swing.JMenuBar;
import javax.swing.JMenuItem;
import javax.swing.event.MenuEvent;
import javax.swing.event.MenuListener;

import Controller.GoogleController;
import fr.gpe.object.Utilisateur;

public class MainWindow extends GoogleView implements ActionListener{
	/**Declaration **/
	private JFrame frame = null;
	private JMenuBar menuBar = new JMenuBar();
	private JMenu menu_home = new JMenu("Accueil");
	private JMenu menu_todolist = new JMenu("TodoList");
	private JMenu menu_plannings = new JMenu("Plannings");
	private JMenu menu_clouds = new JMenu("Clouds");
	private JMenu menu_parametre = new JMenu("Paramètres");
	private JMenuItem menuItem_listFile = new JMenuItem("Liste des fichiers");
	private JMenuItem menuItem_synchro_cloud = new JMenuItem("Synchonisation des clouds");
	private JMenuItem menuItem_syncho_calendar = new JMenuItem("Synchonisation des plannings");
	private Utilisateur u = new Utilisateur ();
		
	public MainWindow(GoogleController controller) {
		super(controller);
		
		u.nom= "FATIER";
		u.prenom = "Ségolène";
		u.login = "fatier_segolene";
		u.mdp = "etna";
		u.token = "yJLkyP3VBKAAAAAAAAAAXPz72gQ_MWAmUeZmYY8lH_ssFo-hTZIEVXS99OYCtooN"; //token de mon compte
		
		frame = new JFrame("GED ETNA");		
		frame.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);	   
		
		//menu_home.add(new JMenuItem("Quitter"));
		menu_home.addMenuListener(new MenuListener() {
			
			public void menuSelected(MenuEvent e) {
				frame.getContentPane().removeAll();//remove
				frame.setContentPane(new HomeView());//add
				frame.revalidate();//refresh ui and layout
				frame.repaint();
			}
			
			public void menuDeselected(MenuEvent e) {
				// TODO Auto-generated method stub
				
			}
			
			public void menuCanceled(MenuEvent e) {
				// TODO Auto-generated method stub
				
			}
		});
		
		menuBar.add(menu_home);
		menuBar.add(menu_todolist);
		menuBar.add(menu_plannings);
		
		menu_clouds.add(menuItem_listFile);
		menu_clouds.addMenuListener(new MenuListener() {
			
			public void menuSelected(MenuEvent e) {
				frame.getContentPane().removeAll();//remove
				frame.setContentPane(new ShowAllFiles(u));;//add
				frame.revalidate();//refresh ui and layout
				frame.repaint();	
			}
			
			public void menuDeselected(MenuEvent e) {
				// TODO Auto-generated method stub
				
			}
			
			public void menuCanceled(MenuEvent e) {
				// TODO Auto-generated method stub
				
			}
		});
		menuBar.add(menu_clouds);
		
		menu_parametre.add(menuItem_synchro_cloud);
		menuItem_synchro_cloud.addActionListener(new ActionListener() {			
			public void actionPerformed(ActionEvent e) {
				frame.getContentPane().removeAll();//remove
				frame.setContentPane(new SynchronisationCloud(new GridLayout(2,2,2,2), u));;//add
				frame.revalidate();//refresh ui and layout
				frame.repaint();	
			}
		});
		menu_parametre.add(menuItem_syncho_calendar);
		menuBar.add(menu_parametre);		
		
		frame.add(new HomeView());
		frame.setJMenuBar(menuBar);
		
		//Dimension de l'écran
		Dimension screenSize = Toolkit.getDefaultToolkit().getScreenSize();
		int width = (int) screenSize.getWidth();
		int height = (int) screenSize.getHeight();
		
	    frame.setSize(width , height);
	    frame.setVisible(true);	   
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
