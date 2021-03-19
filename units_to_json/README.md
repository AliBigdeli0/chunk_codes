# units_to_json
converted important units into JSON files to use as a database or dynamic calculations

# tree structure 
all_units.json file structure header contains : 
```
[
    {
        "id": "unique id", 
        "name": "category name"
        "standards" : [
            {
                "id": "unique id", 
                "name": "standard name , for none standard values this value set to 'default'", 
                "details":[
                    {
                        "id": "unique id", 
                        "name": "unit short name",
                        "full_name" : {
                            "singular": "compelete singular name",
                            "plural": "compelete plural name"
                        }, 
                        "to_anchor": "formula", 
                        "default" : "set true for default value ( optional )"
                    },...
                ]
            } , 
            ...
        ]
    }, 
    ...
]
```

# Attention 
 All ```.json``` files in this project created from ```.js``` files in https://github.com/ben-ng/convert-units 
 
# License
Copyright (c) 2013-2017 Ben Ng and Contributors, http://benng.me

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
