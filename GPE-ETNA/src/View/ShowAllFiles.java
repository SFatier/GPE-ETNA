package View;

import java.awt.Color;
import java.awt.Dimension;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.io.Reader;
import java.net.URL;
import java.nio.charset.Charset;

import javax.swing.ImageIcon;
import javax.swing.JButton;
import javax.swing.JLabel;
import javax.swing.JPanel;
import javax.swing.JTextArea;
import javax.swing.JTextField;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import fr.gpe.object.Utilisateur;

public class ShowAllFiles extends JPanel {
	/**color : 3977A3**/
	private static final long serialVersionUID = 1L;
	private static final String URL_FILE_DROPBOX = "http://localhost:8080/API/dropbox_files";
	private static final String URL_FILE_DROPBOX_SEARCH = "http://localhost:8080/API/Search";
	
	private JTextField tbx_search;
	private JButton btn_search;
	JLabel lbl1;
	
	/**icon**/
	private static ImageIcon icon_word = new ImageIcon("Ressource/pdf.png");
	private static ImageIcon icon_search = new ImageIcon("Ressource/search.png");
	private static ImageIcon icon_not_found = new ImageIcon("Ressource/not_found.png");
	//private static ImageIcon icon_pdf = new ImageIcon("Ressource/word2.jpg");

	/**
	 * Constructeur
	 * */
	public ShowAllFiles(Utilisateur u) {
        if ( u.token != "") {
        	System.out.println("Token enregistré : "+ u.token);        	
        	DrawSearchBar(u);    		
        	DrawFiles(u);
        }else {
        	System.out.println("Pas de token enregistré");
        	JLabel lbl1 = new JLabel("Cloud non synchronisé...");
        	this.add(lbl1);
        	lbl1.setLocation(40, 20);
        	lbl1.setSize(300, 128);        	
        }
        this.setBackground(Color.WHITE);
		this.setLayout(null);		
	}	
	
	/**Dessine la barre de recherche
	 * 
	 */
	private void DrawSearchBar(final Utilisateur u) {
		/**barre de recherche**/
		tbx_search = new JTextField();
		tbx_search.setForeground(Color.BLUE);
		tbx_search.setPreferredSize(new Dimension(150, 30));
		tbx_search.setText("Recherche...");
		this.add(tbx_search);
		tbx_search.setLocation(1150, 20);
		tbx_search.setSize(150, 30);
		
		btn_search = new JButton();
		btn_search.setIcon(icon_search);
		btn_search.addActionListener(new ActionListener() {				
			public void actionPerformed(ActionEvent e) {
				System.out.println("Valeur de la textbox =>" + tbx_search.getText());
				SelectItemSearch(u, tbx_search.getText());
			}
		});
		this.add(btn_search);
		btn_search.setLocation(1300, 20);
		btn_search.setSize(29, 29);
	}

	/**Dessine les fichiers sur l'onglet n°2
	 * 
	 * @param u
	 */
	private void DrawFiles(Utilisateur u) {
		JSONArray json = null;
		int x = 40;
		int y = 100;		
		int x_label = 55;
		int y_label = 260;
		
		try {
			json = readJsonFromUrl(URL_FILE_DROPBOX + "?token="+ u.token);	
		} catch (JSONException e) {
			System.out.println("Erreur : Le JSON n'est pas récupéré.");
		} catch (IOException e) {
			System.out.println("Erreur : Vérifier la connexion");
		}
		final int n = json.length();	
					
		for (int i = 0; i < n; ++i) {
		       final JSONObject person = json.getJSONObject(i);	        
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
		        	lbl_img.setBackground(Color.WHITE);
		        	
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
	}

	/**Vérifie si le nombre d'élément est pair ou impair
	 * 
	 * @param num
	 * @return
	 */
	private boolean isEven(int num) { return ((num % 2) == 0); }
	
	/**Lit le JSON et le renvoi en string
	 * 
	 * @param rd
	 * @return
	 * @throws IOException
	 */
	private static String readAll(Reader rd) throws IOException {
		    StringBuilder sb = new StringBuilder();
		    int cp;
		    while ((cp = rd.read()) != -1) {
		      sb.append((char) cp);
		    }
		    return sb.toString();
		  }

	/**Récupère les données du format JSON
	 * 
	 * @param url
	 * @return
	 * @throws IOException
	 * @throws JSONException
	 */
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
	
	/**Récupère les information de l'item recherché
	 * 
	 */
	public void SelectItemSearch(Utilisateur u, String word) {
		JSONArray json = null;	
		int x = 40;
		int y = 100;		
		int x_label = 55;
		int y_label = 260;
		
		try {
			String URL = URL_FILE_DROPBOX_SEARCH + "?token="+ u.token + "&" + "word=\"" + word + "\"";
			System.out.println(URL);
			json = readJsonFromUrl(URL);
		} catch (JSONException e) {
			System.out.println("ECHEC : Error de récupération du json");
		} catch (IOException e) {
			System.out.println("ECHEC : Error de recherche du fichier");
		}	
		
		final int n = json.length();	
		
		if ( n == 0) {
			this.removeAll();
			DrawSearchBar(u);
        	JLabel lbl1 = new JLabel("\t\tAucun résultat correspondant à la recherche");
        	lbl1.setIcon(icon_not_found);
        	this.add(lbl1);
        	lbl1.setLocation(40, 20);
        	lbl1.setSize(300, 128);  
        	this.repaint();
        	this.revalidate();//refresh ui and layout
		}else {
			this.removeAll();//remove
			for (int i = 0; i < n; ++i) {
			       final JSONObject person = json.getJSONObject(i);	        
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
			        	lbl_img.setBackground(Color.WHITE);
			        	
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
			this.repaint();
        	this.revalidate();//refresh ui and layout
		}
	}
}
