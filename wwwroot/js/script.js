class ClaimManager {
    constructor(context, user) {
        if (context == null) throw new Error("context is null");
        this.Items = [];
        const claims = user.Claims;

        const idTokenJson = context.GetTokenAsync("id_token");
        const accessTokenJson = context.GetTokenAsync("access_token");
        const refreshTokenJson = context.GetTokenAsync("refresh_token");

        this.AddTokenInfo("Refresh Token", refreshTokenJson, true);
        this.AddTokenInfo("Identity Token", idTokenJson);
        this.AddTokenInfo("Access Token", accessTokenJson);
        this.AddTokenInfo("User Claims", claims);
    }

    get AccessToken() {
        if (this.Items == null || this.Items.length == 0) {
            throw new Error("Not tokens found");
        }
        const token = this.Items.find(x => x.Name === "Access Token");
        if (token == null) {
            throw new Error("Not tokens found");
        }

        return token.Token;
    }

    get RefreshToken() {
        if (this.Items == null || this.Items.length == 0) {
            throw new Error("Not tokens found");
        }
        const token = this.Items.find(x => x.Name === "Refresh Token");
        if (token == null) {
            throw new Error("Not tokens found");
        }

        return token.Token;
    }

    AddTokenInfo(nameToken, idTokenJson, skipParsing = false) {
        if (idTokenJson == null || idTokenJson.trim() === "") {
            return;
        }
        this.Items.push(new ClaimViewer(nameToken, idTokenJson, skipParsing));
    }

    AddTokenInfo(nameToken, claims) {
        this.Items.push(new ClaimViewer(nameToken, claims));
    }
}

const ClaimManager = new ClaimManager(context, user);
const AccessToken = ClaimManager.AccessToken();
document.getElementById('GetDetails').addEventListener('click', function () {
    var xhr = new XMLHttpRequest();
    xhr.onreadystatechange = function () {
        if (xhr.readyState === 4 && xhr.status === 200) {
            document.getElementById('NoteCard').innerHTML = xhr.responseText;
        }
    };
    xhr.open('POST', '/Note/GetNoteDetails');
    
    xhr.setRequestHeader('Authorization', 'Bearer ' + AccessToken);
    xhr.send();
});

