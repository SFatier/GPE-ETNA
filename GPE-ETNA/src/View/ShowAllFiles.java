package View;


import java.awt.Color;
import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.io.Reader;
import java.net.URL;
import java.nio.charset.Charset;
import javax.swing.ImageIcon;
import javax.swing.JLabel;
import javax.swing.JPanel;
import javax.swing.JTextArea;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;


public class ShowAllFiles extends JPanel {
	
	public ShowAllFiles() {
		int x = 40;
		int y = 20;		
		int x_label = 55;
		int y_label = 160;
		JSONArray json = null;
		ImageIcon icon_word = new ImageIcon("Ressource/pdf.png");
        ImageIcon icon_pdf = new ImageIcon("Ressource/word2.jpg");
		
		try {
			json = readJsonFromUrl("http://localhost:8080/API/listeFile");
			
		} catch (JSONException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		} catch (IOException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
		final int n = json.length();	
		
		for (int i = 0; i < n; ++i) {
		       final JSONObject person = json.getJSONObject(i);
		        // System.out.println(person.getString("nom"));		        
			   if (isEven(n)) {
					        	
				   //Image
		        	JLabel lbl1 = new JLabel(icon_word);
		        	this.add(lbl1);
		        	lbl1.setLocation(x, y);
		        	lbl1.setSize(128, 128);
		        	
		        	//Texte
		        	JTextArea lbl_img = new JTextArea(person.getString("nom").replace("/", ""));
		        	this.add(lbl_img);
		        	lbl_img.setLocation(x_label, y_label);
		        	lbl_img.setWrapStyleWord(true);
		        	lbl_img.setLineWrap(true);
		        	lbl_img.setSize(100, 80);	  
		        	lbl_img.setBackground(Color.LIGHT_GRAY);
		        	
		        	if (x > 700) {
		        		x = 40;  
		        		y = y + 150 ;
		        		y_label = y_label + 150;
		        	}else {
		        		x = x + 130;
		        		x_label  = x_label + 130;
		        	}
		        }
		}
		this.setLayout(null);
	}	
	
	//Vérifie si le int est pair ou impair
	private boolean isEven(int num) { return ((num % 2) == 0); }
	
	private static String readAll(Reader rd) throws IOException {
		    StringBuilder sb = new StringBuilder();
		    int cp;
		    while ((cp = rd.read()) != -1) {
		      sb.append((char) cp);
		    }
		    return sb.toString();
		  }

	public static JSONArray readJsonFromUrl(String url) throws IOException, JSONException {
		    InputStream is = new URL(url).openStream();
		    try {
		      BufferedReader rd = new BufferedReader(new InputStreamReader(is, Charset.forName("UTF-8")));
		      String jsonText = readAll(rd);
		      JSONObject json = new JSONObject(jsonText);
		      System.out.println(json);
		      JSONArray geodata = json.getJSONArray("record"); 
		      System.out.println(geodata);
		      return geodata;
		    } finally {
		      is.close();
		    }
		  }
}
