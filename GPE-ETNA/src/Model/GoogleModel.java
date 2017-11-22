package Model;

import java.util.List;
import javax.swing.event.EventListenerList;
import Event.GoogleListener;


//Classe et "Ecouteur" pour que d'autre classe puisse accéder aux changements
public class GoogleModel extends File {
	
	private EventListenerList listeners;
	 
	public GoogleModel(List<File> lstFile) {
		super();
		
		listeners = new EventListenerList();
	}
	
	
	public void addGoogleListener(GoogleListener listener){
		listeners.add(GoogleListener.class, listener);
	} 
}
