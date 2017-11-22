package View;

import Controller.GoogleController;
import Event.GoogleListener;


//Indépendant ne se mélange pas à swing
//Elle représente les fonctions de bases d'une vue
public abstract class GoogleView implements GoogleListener{
	private GoogleController controller = null;
 
	public GoogleView(GoogleController controller) {
		this.controller = controller;
	}

	public final GoogleController getController(){
		return controller;
	}
 
	public abstract void display();
	public abstract void close();
}