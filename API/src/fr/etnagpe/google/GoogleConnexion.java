package fr.etnagpe.google;

import org.scribe.builder.ServiceBuilder;
import org.scribe.builder.api.LinkedInApi;
import org.scribe.oauth.OAuthService;
import fr.etnagpe.constant.Constant;
import java.util.Scanner;
import org.scribe.model.OAuthRequest;
import org.scribe.model.Response;
import org.scribe.model.Token;
import org.scribe.model.Verb;
import org.scribe.model.Verifier;

 
public class GoogleConnexion {
  
  private static final String PROTECTED_RESOURCE_URL = "https://www.googleapis.com/oauth2/v2/userinfo?alt=json";
 
  private static final String SCOPE = "https://www.googleapis.com/auth/drive";
 
  private static final Token EMPTY_TOKEN = null;
  private static Scanner in = new Scanner(System.in);
  
  //Google Auth2
  public static OAuthService CreateOAuthService() {
	  OAuthService service = new ServiceBuilder().provider(Google2API.class)
              .apiKey(Constant.GOOGLE_CLIENT_ID).apiSecret(Constant.GOOGLE_CLIENT_SECRET).callback(Constant.GOOGLE_REDIRECT_URL)
              .scope(SCOPE).build();
 
	  	return service;
  }
  
  public static Token GetToken(OAuthService service){ 
	   
      Verifier verifier = null; 
      Token accessToken = null;
 
      // obtient l'url
      String authorizationUrl = service.getAuthorizationUrl(EMPTY_TOKEN);            
      System.out.println("Copier le lien suivant dans le navigateur :" + authorizationUrl); 
  
      //Obtient le code
      System.out.println("Copier le code :");
      System.out.print(">>");
      verifier = new Verifier(in.nextLine());
      System.out.println();
 
      //Récupère le token
      System.out.println("Trading the Request Token for an Access Token...");
      accessToken = service.getAccessToken(EMPTY_TOKEN, verifier);
      System.out.println("Token: " + accessToken );
      return accessToken;
  }
  
  public static void GetInfos(OAuthService service) {
	  OAuthRequest request = new OAuthRequest(Verb.GET,
              PROTECTED_RESOURCE_URL);
      service.signRequest(GetToken(service), request);
      Response response = request.send();
      System.out.println("Got it! Lets see what we found...");
      System.out.println();
      System.out.println(response.getCode());
      System.out.println(response.getBody());
      in.close();
  }
  
}
