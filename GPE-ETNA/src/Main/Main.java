package Main;

import Controller.GoogleController;
import Model.GoogleModel;

public class Main {
    public static void main(String[] args) {
        //System.out.println("Hello world!");
        
        //test -- on peut ne pas le mettre
//        List<File> lst = new ArrayList<File>();
//        File file = new File();
//        file.FileName = "toto";
//        file.ID_File = "iesmoifhsmgoiher";
//        lst.add(file);
        
        //appelle le model puis le affiche la fenetre demandé par le controller
        GoogleModel model = new GoogleModel();
		GoogleController controller = new GoogleController(model);
		controller.displayViews();
    }
}
