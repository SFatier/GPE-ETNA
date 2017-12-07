package View;

import java.awt.Color;

import javax.swing.JPanel;

import org.jfree.chart.ChartFactory;
import org.jfree.chart.ChartPanel;
import org.jfree.chart.JFreeChart;
import org.jfree.data.general.DefaultPieDataset;

public class HomeView extends JPanel {
	
	private static final long serialVersionUID = 1L;

	public HomeView() {
		JPanel content = new JPanel();
		
		DefaultPieDataset pieDataset = new DefaultPieDataset(); 
	    pieDataset.setValue("Valeur1", new Integer(27)); 
	    pieDataset.setValue("Valeur2", new Integer(10)); 
	    pieDataset.setValue("Valeur3", new Integer(50)); 
	    pieDataset.setValue("Valeur4", new Integer(5)); 

	    JFreeChart pieChart = ChartFactory.createPieChart("Test 1", pieDataset, true, true, true); 
	    pieChart.createBufferedImage(100, 100);
	    ChartPanel cPanel = new ChartPanel(pieChart); 
	    cPanel.setLocation(0, 200);
	    cPanel.setSize(80, 80);
		content.add(cPanel);
		
		
		DefaultPieDataset pieDataset2 = new DefaultPieDataset(); 
		pieDataset2.setValue("Valeur1", new Integer(50)); 
		pieDataset2.setValue("Valeur2", new Integer(30)); 
		pieDataset2.setValue("Valeur3", new Integer(10)); 
		pieDataset2.setValue("Valeur4", new Integer(10)); 

	    JFreeChart pieChart2 = ChartFactory.createPieChart("Test 2", pieDataset2, true, true, true);
	    pieChart2.createBufferedImage(100, 100);
	    ChartPanel cPanel2 = new ChartPanel(pieChart2); 
	    cPanel2.setLocation(50, 200);
	    cPanel2.setSize(80, 80);
	    content.add(cPanel2);
	    
	    content.setSize(700, 500);
	    this.setBackground(Color.white);
	    add(content);
	}
}
