package View;

import java.awt.Color;
import java.awt.Font;
import java.awt.Graphics;
import javax.swing.JPanel;

public class Panneau extends JPanel{
  private Color color = Color.white;
  private static int COUNT = 0;
  private String message = "";
   
  public Panneau(){}
  public Panneau(Color color){
    this.color = color;
    this.message = "Contenu du panneau N°" + (++COUNT);
  }
  public void paintComponent(Graphics g){
    g.setColor(this.color);
    g.fillRect(0, 0, this.getWidth(), this.getHeight());
    g.setColor(Color.white);
    g.setFont(new Font("Arial", Font.BOLD, 15));
    g.drawString(this.message, 10, 20);
  }
}