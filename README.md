# Assingment Scalable Web

API Rest to diff data asynchronously. Built using asp.net core 2.0/Microsoft Misual Studio Community Edition.

Usage:

1 - Post data to be diff-ed on left side
POST /v1/diff/{id}/left 

2 - Post data to be diff-ed on right side
POST /v1/diff/{id}/right

3 - Execute and Get the Diff Results
GET /v1/diff/{id}

Parameters
{id} - Diff Request identity, used to insert diff data for both sides, and retrieve the results later.

For POST operations, the data to be diff-ed needs to be send on POST request body, with application/json Content-Type.
Examples:

"IHsgIm5hbWUiOiJKb2huIiwgImFnZSI6MzAsICJjYXIiOm51bGwgfQ=="
(Base64 of { "name":"John", "age":30, "car":null })

"IHsgIm5hbWUiOiJQZXRlIiwgImFnZSI6MzAsICJjYXIiOm51bGwgfQ=="
(Base64 of { "name":"Pete", "age":30, "car":null })

Results Example:

{
    "id": 1,
    "left": "IHsgIm5hbWUiOiJKb2huIiwgImFnZSI6MzAsICJjYXIiOm51bGwgfQ==",
    "right": "IHsgIm5hbWUiOiJQZXRlIiwgImFnZSI6MzAsICJjYXIiOm51bGwgfQ==",
    "areEqual": false,
    "haveSameSize": true,
    "diffInsights": [
        "Difference detected, starting at offset 16 with length of 5."
    ]
}

API Demo:
http://leandromedeirosasw.azurewebsites.net

API Documentation/Explorer: 
http://leandromedeirosasw.azurewebsites.net/swagger

Postman collection:
https://www.getpostman.com/collections/41034cb58596eb128dc0

Suggestions for improvement:

- For a real world scenario with a huge number of requests, I would suggest to use a Session/State storage model like "Redis" instead a
SQL storage model. The diff data could be stored the key-value entries, setting a TTL.

- To improve the diff feature, I would suggest a change to show the actual diffs on the insights, changing the code in GetDiffInsights 
method:

      var leftDifferenceBuilder = new StringBuilder();
      var rightDifferenceBuilder = new StringBuilder();

      while (position < left.Length && left[position] != right[position])
      {
        leftDifferenceBuilder.Append(left[position]);
        rightDifferenceBuilder.Append(right[position]);

        position++;
      }

      if (leftDifferenceBuilder.Length > 0)
        result.Add($"Starting at offset {position}, the left data has the value '{leftDifferenceBuilder}', and right data has '{rightDifferenceBuilder}'.");

