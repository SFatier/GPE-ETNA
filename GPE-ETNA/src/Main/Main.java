package Main;

import Controller.GoogleController;
import Model.GoogleModel;

public class Main {
    public static void main(String[] args) {
    	/*Test de la suppresion d'une extension dans le nom fichier*/
    	String str = "Soft War.png".replace(" ", "");
    	String nomCourt = (str != null) ? str.substring(0,str.indexOf('.')) : "";
    	System.out.println(nomCourt.toLowerCase());
    	
//    	String extension = "";
//		String fileName = "CCCCCCC.png";
//		int i = fileName.lastIndexOf('.');
//		if (i > 0) {
//		    extension = fileName.substring(i+1);
//		}
//		System.out.println(extension);
    	
        //appelle le model puis le affiche la fenetre demandé par le controller
        GoogleModel model = new GoogleModel();
		GoogleController controller = new GoogleController(model);
		controller.displayViews();
    }
}
