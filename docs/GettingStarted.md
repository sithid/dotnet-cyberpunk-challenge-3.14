# Notes
Walkthrough notes on what needs to happen to make the KuangPrimusMalware variant for a 3rd type, Biotechnica.

1. [Setting up the Data](#setting-up-the-data-model)
2. [Adding the helper utility](#utilities)
3. [Adapting the Server Connection](#serverconnection)
4. [Adding Biotechnica to our Generic MultiKaung malware](#multi-kuang-primus-malware)
5. [Adding the Biotechnica-ONLY Kuang malware](#biotechnica-kuang-primus-malware)
6. [Techniques](#opportunities-to-learn)
7. [Bonus - *Optional](#bonus)

### Setting up the data model
- Go to `_HttpResponseModels.cs` and make models to represent each part of the message from the biotechnica.
	- Investigate the samples of data and compare to the Arasaka models and/or Militech models to see what you need to do
	- There will 3 models you need to make:
		- `<blank>MesageRoot` - The whole message we get back from the API
		- `<blank>ProcessList` - Just the part of the MessageRoot that contains process list
		- `<blank>MessageSystem` - Just the part of MessageRoot that contains info about the connection
			- Militech and Arasaka name this a little differently, e.g. "MessageConnection", "MessageContent". The properties listed here (refer to the sampel data for reference) will be all the properties that the `system` dictionary can contain across all three types of messages. Since the `biotechnica-connection-established.json` sample data dictionary has the `message` property on the `system` dictionary and the `biotechnica-model-type.json` sample has the `model` property on `system` then your `BiotechnicaMessageSystem` class should have both of those properties set; more below...
			- Here's why that's okay: When C# tries to map the json from the HTTP Request to `BiotechnicaMessageSystem` then it'll only map the properties of the class to the properties in the JSON that exist. For example, if the JSON doesn't have a `model` property on `system` but it does have a `message` property then over on the class the C# will map the `model` property to `null` and map the `message` property to whatever the value is.
- Ensure that your `BiotechnicaMessageRoot` inherits from `MessageRoot`.
- Same goes for the `BiotechnicaProcessList` inheriting from `ProcessList`
- You're done!


### Utilities
- Go to `utilities.cs`.
- In the `static class Helpers {}` there are helper functions like `ToArasakaMessage()` and `ToMilitechMessage()`. We need the exact same functionality except we'll be using the type `BiotechnicaMessageRoot` instead of `ArasakaMessageRoot`. 
    - ***Note for the curious***: You might think that it makes sense to genericize these methods instead of having 3 separate `ToArasaka...()`, `ToBiotechnica...()`, and `ToMilitech...()` methods but because these are static we can't do that. Might be able to pull some trickery by passing in the type as a parameter but that's way too much work and little chance of (well-understood) success.
- You're done!

### ServerConnection
- Go to `ServerConnection.cs`. This class is the generic object that represents a connection to a specific server (Arasaka/Militech/Biotechnica). This gets set on a malware class when the `Initialize()` method runs. As such we need to add additional conditions to allow for getting a Biotechnica ServerConnection.
- Add an else-if conditional in the public `SendRequest()` method
	- In that conditional it needs to set `fullMessagePath = $"biotechnica/{uriPath}";`  
    This allows requests to always include `<api-address>/biotechnica/` in any request we make.
	- While you're at it go ahead and add an `else` condition that will `throw new NotImplementedException`. By doing this we ensure that we can never accidentally send a request to somthing we don't support. The original author forgot to do this ;)
- Add an else-if conditional in the protected `_SendRequest()`
	- For the condition itself we need to test, like we're doing for `ArasakaMessageRoot` and `MilitechMessageRoot`, that the typeof T is == to the typeof `BiotechnicaMessageRoot`.
	- In the body of that conditional we need to do:
	  `return (T)Convert.ChangeType(jsonResponse.ToBiotechnicaMessage(), typeof(T));`
	  This will convert the jsonResponse to the typeof T.  
      For more details on this: That confusing looking line is just saying "Cast the result of 'Changing the json to Biotechnica message to T' to the type T"
- You're done!


### Multi Kuang Primus Malware
- Go to `MultiKuangPrimusMalware.cs`. This is the generic multi-handler for all the server types. We can use this to spin up malware for all of them. Not just ***anyone*** can get access to this.
- Add a else if for `GetIceTypeOnRemote()`
- Add an else if for `_GetProcessList()` to handle the new `BiotechnicaMessageRoot` (or whatever you called it) and the new `BiotechnicaProcessList` (or whatever you called it).
- You'll notice in `GetIceTypeOnRemote()` and `_GetProcessList` that the properties on the MessageRoot object are different. For example, in `MilitechKuangPrimusMalware.cs` in the `GetIceTypeOnRemote()` method it uses `militechMessageResponse.connection.model` but `connection` only exists on the `MilitechMessageRoot` class, not on the Arasaka or Biotechnica variants. You'll need to refer to the class see what the right property is :)

### Biotechnica Kuang Primus Malware
- Create new class called `BiotechnicaKuangPrimusMalware.cs` in the `malware/` directory. This is preferred but as long as your namespace on this class is `namespace dotnet_cyberpunk_challenge_3_14.malware` then it'll work. You can refer to how the Arasaka and Militech variants are built out if you're unsure.
- This class will need to inhert from `MultiKuangDaemonFamilyBase<>` and you'll need to pass in the `<blank>MessageRoot` and `<blank>ProcessList` classes you created for Biotechnica.
- You'll need to implement all the abstract methods. Your options for doing so: 
	- Quick way to do this is to use the Visual Studio/VSCode "Quick Fix" option on the red squigglies to `Fix: implement abstract class`. 
	- Or you can manually type out all the methods. 
	- OR simply copy all the methods in `MilitechKuangPrimusMalware` and ensure it only works with Biotechnica (quickest)
	- ***Or you can just copy either the Arasaka or MilitechKuangPrimusMalware.cs*** file and make changes to the copy so that it only works with Biotechnica. 
- Change any mention of Militech/Arasaka to Biotechnica if those mentions exist.
- One thing you'll notice if you copied code from the Arasaka or Militech versions is that in `GetIceTypeOnRemote()` and `_GetProcessList` is that the properties on the MessageRoot object are different. For example, in `MilitechKuangPrimusMalware.cs` in the `GetIceTypeOnRemote()` method it uses `militechMessageResponse.connection.model` but `connection` only exists on the `MilitechMessageRoot` class, not on the Arasaka or Biotechnica variants. You'll need to refer to the class see what the right property is :)

### Opportunities to learn
- Why do we setup the `protected ... _GetProcessList()` in the malware classes? Check out the Base class where you'll find the `public` method `GetProcessList()` which is going to execute the protected override of `_GetProcessList()`. The reasoning is: The way that we use the Arasaka, Militech, or Biotechnica malware to get the process list is the same (by using the public `GetProcessList()`...but the way each of those classes implement how that process list is grabbed is their responsibility (by using their own implementation of `_GetProcessList()`)

#### Bonus
***For the brave** (masochistic)*   
If you feel super brave then you can figure out how to make `SetupIceBreakerTunnelToTarget()` work in Arasaka+Militech+Biotechnica variants of the KuangPrimusMalware. It should work fine in the `MultiKuangPrimusMalware` but more work is needed on the individual variants of the malware and I was already brain-fried at the time to figure it out :shrug: Up to you!