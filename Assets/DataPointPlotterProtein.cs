using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataPointPlotterProtein: MonoBehaviour
{
    public string inputFile;

    public GameObject DataPointPrefab;
    public GameObject DataPointHolderProtein;
    private List<Dictionary<string, object>> dataPointList;

    public int columnScale = 5;
    public int columnX = 4;
    public int columnY = 1;
    public int columnZ = 0;

    public string ScaleName;
    public string xName;
    public string yName;
    public string zName;

    public float plotScale = 10;

    private void Start()
    {
        dataPointList = CSVReader.Read(inputFile);

        List<string> columnList = new List<string>(dataPointList[1].Keys);

        xName = columnList[columnX];
        yName = columnList[columnY];
        zName = columnList[columnZ];
        ScaleName = columnList[columnScale];

        float xMax = FindMaxValue(xName);
        float yMax = FindMaxValue(yName);
        float zMax = FindMaxValue(zName);
        float ScaleMax = FindMaxValue(ScaleName);

        float xMin = FindMinValue(xName);
        float yMin = FindMinValue(yName);
        float zMin = FindMinValue(zName);
        float ScaleMin = FindMinValue(ScaleName);


        for (var i = 0; i < dataPointList.Count; i++)
        {
            float x = ((System.Convert.ToSingle(dataPointList[i][xName]) - xMin) / (xMax - xMin)) +2;
            float y = (System.Convert.ToSingle(dataPointList[i][yName]) - yMin) / (yMax - yMin);
            float z = (System.Convert.ToSingle(dataPointList[i][zName]) - zMin) / (zMax - zMin);
            float Scale = (System.Convert.ToSingle(dataPointList[i][ScaleName]) - ScaleMin) / (ScaleMax - ScaleMin); 

            GameObject dataPoint = Instantiate(
                    DataPointPrefab,
                    new Vector3(x, y, z)*plotScale,
                    Quaternion.identity);

            dataPoint.transform.parent = DataPointHolderProtein.transform;
            dataPoint.transform.localScale += new Vector3(Scale, Scale, Scale);
            //scales the data points by their x, y and z variables.

            string dataPointName =
                dataPointList[i][xName] + " "
                + dataPointList[i][yName] + " "
                + dataPointList[i][zName];

            dataPoint.transform.name = dataPointName;

            dataPoint.GetComponent<Renderer>().material.color =
                new Color(x, y, z, 1.0f);

        }
    }

    private float FindMaxValue(string columnName)
    {
        float maxValue = Convert.ToSingle(dataPointList[0][columnName]);

        for(var i = 0; i < dataPointList.Count; i++)
        {
            if (maxValue < Convert.ToSingle(dataPointList[i][columnName])) 
                maxValue = Convert.ToSingle(dataPointList[i][columnName]);
        }

        return maxValue;
    }

    private float FindMinValue(string columnName)
    {
        float minValue = Convert.ToSingle(dataPointList[0][columnName]);

        for(var i = 0; i < dataPointList.Count; i++)
        {
            if (Convert.ToSingle(dataPointList[i][columnName]) < minValue)
                minValue = Convert.ToSingle(dataPointList[i][columnName]);
        }

        return minValue;
    }
}
