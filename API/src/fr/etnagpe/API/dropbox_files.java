package fr.etnagpe.API;

import java.io.IOException;
import java.io.PrintWriter;

import javax.servlet.ServletException;
import javax.servlet.annotation.WebServlet;
import javax.servlet.http.HttpServlet;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;

import com.dropbox.core.DbxException;
import com.dropbox.core.v2.DbxClientV2;
import com.dropbox.core.v2.files.ListFolderErrorException;
import com.dropbox.core.v2.files.ListFolderResult;
import com.dropbox.core.v2.files.Metadata;

import fr.etnagpe.dropbox.DropBoxConnexion;

/**
 * Servlet implementation class dropbox_files
 */
@WebServlet("/dropbox_files")
public class dropbox_files extends HttpServlet {
	private static final long serialVersionUID = 1L;
       
    /**
     * @see HttpServlet#HttpServlet()
     */
    public dropbox_files() {
        super();
        // TODO Auto-generated constructor stub
    }

	/**
	 * @see HttpServlet#doGet(HttpServletRequest request, HttpServletResponse response)
	 */
	protected void doGet(HttpServletRequest request, HttpServletResponse response) throws ServletException, IOException {
		ListFolderResult rslt = null;
		String token = request.getParameter("token");
		DropBoxConnexion dc = new DropBoxConnexion();
		DbxClientV2 client = dc.ConnectClient(token);	
		int i = 0;
		String strjson = "{\"record\": [";
		
		response.setContentType("application/json");
		PrintWriter writer = response.getWriter();
		//Récupère la liste des fichiers client
		try {
			rslt = dc.ListFiles(client);
		} catch (ListFolderErrorException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		} catch (DbxException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
				
		 for (Metadata metadata : rslt.getEntries()) {
             // writer.println( metadata.getPathLower()) ;
             if (i < (rslt.getEntries().size() - 1)) {
            	 strjson += "{\"pathDisplay\":\""+ metadata.getPathDisplay() + "\",";
            	 strjson += "\"nom\":\""+ metadata.getName() + "\",";
            	 strjson += "\"type_icone\":\""+ dc.GetTypeMime(metadata.getName()) + "\",";
            	 strjson += "\"parentSharedFolderId\":\""+ metadata.getParentSharedFolderId() + "\",";
            	 strjson += "\"pathLower\":\""+ metadata.getPathLower() + "\" },";
             }else {
            	 strjson += "{\"pathDisplay\":\""+ metadata.getPathDisplay() + "\",";
            	 strjson += "\"nom\":\""+ metadata.getName() + "\",";
            	 strjson += "\"type_icone\":\""+ dc.GetTypeMime(metadata.getName())  + "\",";
            	 strjson += "\"parentSharedFolderId\":\""+ metadata.getParentSharedFolderId() + "\",";
            	 strjson += "\"pathLower\":\""+ metadata.getPathLower() + "\" }";
             }
             i++;
         }
		 		 
		 strjson += "]}";
		
		 writer.print(strjson);
		 writer.flush();
	}

	/**
	 * @see HttpServlet#doPost(HttpServletRequest request, HttpServletResponse response)
	 */
	protected void doPost(HttpServletRequest request, HttpServletResponse response) throws ServletException, IOException {
		// TODO Auto-generated method stub
		doGet(request, response);
	}

}
