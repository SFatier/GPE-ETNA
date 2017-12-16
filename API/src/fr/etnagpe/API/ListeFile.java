package fr.etnagpe.API;

import java.io.IOException;
import java.io.PrintWriter;
import javax.servlet.ServletException;
import javax.servlet.annotation.WebServlet;
import javax.servlet.http.HttpServlet;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;
import com.dropbox.core.DbxApiException;
import com.dropbox.core.DbxException;
import com.dropbox.core.v2.DbxClientV2;
import com.dropbox.core.v2.files.FileMetadata;
import com.dropbox.core.v2.files.ListFolderErrorException;
import com.dropbox.core.v2.files.ListFolderResult;
import com.dropbox.core.v2.files.Metadata;

import fr.etnagpe.dropbox.DropBoxConnexion;
	/**
 * Servlet implementation class ListeFile
 */
@WebServlet("/listeFile")
public class ListeFile extends HttpServlet {
	private static final long serialVersionUID = 1L;
	private String token; //laisse en m�moire le token
	private String user = "";
       
    /**
     * @see HttpServlet#HttpServlet()
     */
    public ListeFile() {
        super();
        // TODO Auto-generated constructor stub
    }

	/**
	 * @see HttpServlet#doGet(HttpServletRequest request, HttpServletResponse response)
	 * Retourne un JSON
	 */
	protected void doGet(HttpServletRequest request, HttpServletResponse response) throws ServletException, IOException {				
		DropBoxConnexion dc = new DropBoxConnexion();
		System.out.println(token);
		if (token == "" || token == null) {
			token = dc.GetToken();
		}
		DbxClientV2 client = dc.ConnectClient(token);		
		
		//R�cup�re les informations du client
		try {
			user = dc.GetCurrentAccount(client);
		} catch (DbxApiException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		} catch (DbxException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}

		PrintWriter writer = response.getWriter();
		
		 writer.print("");
		 writer.flush();
	}
}
