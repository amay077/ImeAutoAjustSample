package amay077.net.androidimestretchsample;

import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.widget.ArrayAdapter;
import android.widget.ListView;
import android.widget.SimpleAdapter;

public class ChatStyleActivity extends AppCompatActivity {

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_chat_style);

        ListView listView = (ListView) findViewById(R.id.listView);

        String[] members = {
                "0001",
                "0002",
                "0003",
                "0004",
                "0005",
                "0006",
                "0007",
                "0008",
                "0009",
                "0010",
                "0011",
                "0012",
                "0013",
                "0014",
                "0015",
                "0016",
                "0017",
                "0018",
                "0019",
                "0020"
        };

        ArrayAdapter<String> adapter = new ArrayAdapter<String>(this,
                android.R.layout.simple_expandable_list_item_1, members);
        listView.setAdapter(adapter);
    }
}
