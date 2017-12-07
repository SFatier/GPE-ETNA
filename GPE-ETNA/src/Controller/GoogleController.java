package Controller;

import Model.GoogleModel;
import View.GoogleView;
import View.MainWindow;


//Controlle la fenetre de test de google
public class GoogleController {

	public GoogleView view = null;
 
	private GoogleModel model = null;
 
	public GoogleController (GoogleModel model){
		this.model = model;
 
		view = new MainWindow(this);  //new JFrameGoogle(this);
		addListenersToModel();
	}
 
	//Ajoute un listener pour notifier un evenement
	private void addListenersToModel() {
		model.addGoogleListener(view);
	}
 
	//Affiche la fenetre
	public void displayViews(){
		view.display();
	}
	
	//Ferme la fenetre
	public void closeViews(){
		view.close();
	}
}