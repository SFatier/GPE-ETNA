package View;

import java.awt.Color;
import java.awt.Dimension;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.awt.event.MouseAdapter;
import java.awt.event.MouseEvent;
import java.awt.event.MouseListener;
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
	/**
	 * color : 3977A3*
	 * url icon => https://www.flaticon.com/free-icon/sad-suffering-crying-emoticon-face_42779#term=cry&page=1&position=32
	 * https://www.iconfinder.com/icons/79356/powerpoint_icon#size=128
	 * */
	private static final long serialVersionUID = 1L;
	private static final String URL_FILE_DROPBOX = "http://localhost:8080/API/dropbox_files";
	private static final String URL_FILE_DROPBOX_SEARCH = "http://localhost:8080/API/Search";
	
	private JTextField tbx_search;
	private JButton btn_search;
	JLabel lbl1, label_refresh, label_filter;
	
	/**icon**/
	private static ImageIcon icon_word = new ImageIcon("Ressource/word.png");
	private static ImageIcon icon_search = new ImageIcon("Ressource/search.png");
	private static ImageIcon icon_not_found = new ImageIcon("Ressource/not_found.png");
	private static ImageIcon icon_refresh = new ImageIcon("Ressource/icon_refresh.png");
	private static ImageIcon icon_filter = new ImageIcon("Ressource/icon_filter.png");
	private static ImageIcon icon_pdf = new ImageIcon("Ressource/pdf.png");
	private static ImageIcon icon_excel = new ImageIcon("Ressource/excel.png");
	private static ImageIcon icon_image = new ImageIcon("Ressource/image.png");
	private static ImageIcon icon_archive = new ImageIcon("Ressource/archive.png");
	private static ImageIcon icon_audio = new ImageIcon("Ressource/music.png");
	private static ImageIcon icon_video = new ImageIcon("Ressource/video.png");
	private static ImageIcon icon_code = new ImageIcon("Ressource/code.png");
	private static ImageIcon icon_powerpoint = new ImageIcon("Ressource/powerpoint.png");
	private static ImageIcon icon_text = new ImageIcon("Ressource/text.png");
	private static ImageIcon icon_folder = new ImageIcon("Ressource/folder.png");
	
	/**
	 * Constructeur
	 * */
	public ShowAllFiles(Utilisateur u) {
        CreatePanel(u);
        this.setBackground(Color.WHITE);
		this.setLayout(null);		
	}	
	
	/**
	 * Creation du Panel
	 */
	private void CreatePanel(Utilisateur u) {
		 if ( u.token != "") {
	        	DrawSearchBar(u); 
	        	DrawFilters(u);
	        	DrawFiles(u);
	        }else {
	        	JLabel lbl1 = new JLabel("Cloud non synchronisé...");
	        	this.add(lbl1);
	        	lbl1.setLocation(40, 20);
	        	lbl1.setSize(300, 128);        	
	        }
	}

	private void DrawFilters(final Utilisateur u) {
		label_refresh = new JLabel(icon_refresh);
		label_refresh.setToolTipText("Rafaîchir les fichiers");
		label_refresh.addMouseListener(new MouseListener() {
			
			public void mouseReleased(MouseEvent e) {
				// TODO Auto-generated method stub
				
			}
			
			public void mousePressed(MouseEvent e) {
				// TODO Auto-generated method stub
				
			}
			
			public void mouseExited(MouseEvent e) {
				// TODO Auto-generated method stub
				
			}
			
			public void mouseEntered(MouseEvent e) {
				// TODO Auto-generated method stub
				
			}
			
			public void mouseClicked(MouseEvent e) {
				//Raffraichir le panel 
				removeAll();
				CreatePanel(u);
				repaint();
		    	revalidate();//refresh ui and layout
			}
		});
		
		add(label_refresh);
		label_refresh.setLocation(30, 50);
		label_refresh.setSize(30, 30);
		
		label_filter = new JLabel(icon_filter);
		label_filter.setToolTipText("Filtrer par cloud");
		label_filter.addMouseListener(new MouseListener() {			
			public void mouseClicked(MouseEvent e) {
				//Si clique sur refresh on récupère à nouveau les fichiers
		
			}
		
			public void mousePressed(MouseEvent e) {
				// TODO Auto-generated method stub
				
			}
		
			public void mouseReleased(MouseEvent e) {
				// TODO Auto-generated method stub
				
			}
		
			public void mouseEntered(MouseEvent e) {
				// TODO Auto-generated method stub
				
			}
		
			public void mouseExited(MouseEvent e) {
				System.out.println("Non implémenté");
			}
		});
		
		add(label_filter);
		label_filter.setLocation(60, 47);
		label_filter.setSize(40, 40);
	}

	/**Dessine la barre de recherche
	 * 
	 */
	private void DrawSearchBar(final Utilisateur u) {
		tbx_search = new JTextField();
		tbx_search.setForeground(Color.BLUE);
		tbx_search.setPreferredSize(new Dimension(150, 30));
		tbx_search.setText("Recherche...");
		this.add(tbx_search);
		tbx_search.setLocation(1150, 20);
		tbx_search.setSize(150, 30);
		//Supprime l'ancien texte dans la textbox
		tbx_search.addMouseListener(new MouseAdapter(){
            @Override
            public void mouseClicked(MouseEvent e){
            	tbx_search.setText("");
            }
        });
		
		
		btn_search = new JButton();
		btn_search.setIcon(icon_search);
		btn_search.addActionListener(new ActionListener() {				
			public void actionPerformed(ActionEvent e) {
				System.out.println("Rechercher le document sous le nom de :" + tbx_search.getText());
				SelectItemSearch(u, tbx_search.getText());
			}
		});
		this.add(btn_search);
		btn_search.setLocation(1300, 20);
		btn_search.setSize(29, 29);
	}

	/**Dessine les fichiers récupérer dans le JSON
	 * 
	 * @param u
	 */
	private void DrawFiles(Utilisateur u) {
		JSONArray json = null;
		int x = 60;
		int y = 100;		
		int x_label = 75;
		int y_label = 260;
		
		try {
			json = readJsonFromUrl(URL_FILE_DROPBOX + "?token="+ u.token);	
			System.out.println(json);
			final int n = json.length();	
			
			for (int i = 0; i < n; ++i) {
			       final JSONObject person = json.getJSONObject(i);	 
			       
			       ImageIcon ii = GetIcon(person.getString("type_icone"));
			       
				   //if (isEven(n)) {					        	
					   //Image
			        	JLabel lbl1 = new JLabel(ii);
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
			        	
			        	if (x > 1000) {
			        		x = 40;  
			        		y = y + 250 ;
			        		x_label = 40;
			        		y_label = y + 150;
			        	}else {
			        		x = x + 130;
			        		x_label  = x_label + 130;
			        	}
			       // }
			}
		} catch (JSONException e) {
			System.out.println("Erreur : Le JSON n'est pas récupéré.");
		} catch (IOException e) {
			System.out.println("Erreur : Vérifier la connexion ou vérifier que l'API est run.");
		}		
	}

	private ImageIcon GetIcon(String str) {
		ImageIcon icone =  null;
		
		switch (str)
		{
			case "image" : icone = icon_image;break;
			case "archive" : icone = icon_archive;break;
			case "audio" : icone = icon_audio;break;
			case "video" : icone = icon_video;break;
			case "code" : icone = icon_code;break;
			case "excel" : icone = icon_excel;break;
			case "pdf" : icone = icon_pdf;break;
			case "powerpoint" : icone = icon_powerpoint;break;
			case "word" : icone = icon_word;break;
			case "text" : icone = icon_text;break;	
			case "folder" : icone = icon_folder;break;		
		}
		return icone;		
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
		      JSONArray geodata = json.getJSONArray("record");
		      return geodata;
		    } finally {
		      is.close();
		    }
		  }
	
	/**
	 * Récupère les information de l'item recherché
	 * @param u
	 * @param word
	 */
	public void SelectItemSearch(Utilisateur u, String word) {
		JSONArray json = null;	
		int x = 40;
		int y = 100;		
		int x_label = 55;
		int y_label = 260;
		
		try {
			String URL = URL_FILE_DROPBOX_SEARCH + "?token="+ u.token + "&" + "word=" + word.replace(" ", "");
			System.out.println(URL);
			json = readJsonFromUrl(URL);
			System.out.println(json);
			final int n = json.length();				
			this.removeAll();
			DrawSearchBar(u);
			DrawFilters(u);
			
			if ( n == 0) {
	        	JLabel lbl1 = new JLabel("Aucun résultat correspondant à la recherche. \n ");
	        	lbl1.setIcon(icon_not_found);
	        	this.add(lbl1);
	        	lbl1.setLocation(40, 80);
	        	lbl1.setSize(300, 128); 
			}else {		
				for (int i = 0; i < n; ++i) {
				       final JSONObject person = json.getJSONObject(i);	        
				       ImageIcon ii = GetIcon(person.getString("type_icone"));
										        	
						   //Image
				        	JLabel lbl1 = new JLabel(ii);
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
		} catch (JSONException e) {
			System.out.println("ECHEC : Error de récupération du json" + e.getMessage());
		} catch (IOException e) {
			System.out.println("ECHEC : Error de recherche du fichier");
		}	
	}
}
